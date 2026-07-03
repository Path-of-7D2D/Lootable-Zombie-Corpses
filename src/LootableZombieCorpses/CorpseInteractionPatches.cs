using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine.Scripting;

namespace LootableZombieCorpses
{
    internal static class CorpseInteraction
    {
        internal const string Command = "search";
        private const string MarkerProperty = "LootableCorpse";
        private static readonly MethodInfo LockRequestMethod = typeof(LockManager).GetMethod(
            "LockRequestLocal",
            new[] { typeof(ILockTarget), typeof(ILockContext), typeof(ushort) });

        internal static bool IsRuntimeReady => LockRequestMethod != null;

        internal static bool IsSupported(Entity entity)
        {
            if (!(entity is EntityZombie))
            {
                return false;
            }

            EntityClass entityClass = EntityClass.list[entity.entityClass];
            return entityClass.Properties.Values.TryGetValue(MarkerProperty, out string value)
                && string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
        }

        internal static bool CanSearch(Entity entity)
        {
            return IsSupported(entity) && entity.IsDead() && EnsureBag(entity);
        }

        internal static bool IsSearchCommand(string command)
        {
            return string.Equals(command, Command, StringComparison.Ordinal);
        }

        internal static bool IsSearchContext(ILockContext context)
        {
            return context is Entity.EntityLockContext entityContext
                && IsSearchCommand(entityContext.Command);
        }

        internal static bool EnsureBag(Entity entity)
        {
            if (entity == null)
            {
                return false;
            }

            if (entity.bag != null)
            {
                return true;
            }

            try
            {
                entity.InitializeBagFromLootList();
            }
            catch (Exception ex)
            {
                Log.Warning(string.Format(
                    "[LootableZombieCorpses] Failed to initialize corpse loot bag for {0}: {1}",
                    entity.GetDebugName(),
                    ex));
            }

            return entity.bag != null;
        }

        internal static void RequestLock(Entity entity, Entity.EntityLockContext context)
        {
            if (LockRequestMethod == null)
            {
                Log.Error("[LootableZombieCorpses] Could not find the V3 entity lock method.");
                return;
            }

            try
            {
                LockRequestMethod.Invoke(LockManager.Instance, new object[] { entity, context, (ushort)0 });
            }
            catch (TargetInvocationException ex)
            {
                Log.Error(string.Format(
                    "[LootableZombieCorpses] Corpse lock request failed: {0}",
                    ex.InnerException ?? ex));
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(
                    "[LootableZombieCorpses] Corpse lock request failed: {0}",
                    ex));
            }
        }
    }

    [Preserve]
    [HarmonyPatch(typeof(Entity), nameof(Entity.GetActivationCommands))]
    internal static class CorpseCommandListPatch
    {
        [Preserve]
        private static void Postfix(Entity __instance, ref EntityActivationCommand[] __result)
        {
            if (!CorpseInteraction.CanSearch(__instance))
            {
                return;
            }

            for (int i = 0; __result != null && i < __result.Length; i++)
            {
                if (CorpseInteraction.IsSearchCommand(__result[i].commandId))
                {
                    return;
                }
            }

            int count = __result?.Length ?? 0;
            EntityActivationCommand[] commands = new EntityActivationCommand[count + 1];
            if (count > 0)
            {
                Array.Copy(__result, commands, count);
            }

            commands[count] = new EntityActivationCommand(
                CorpseInteraction.Command,
                CorpseInteraction.Command);
            __instance.activationCommands = commands;
            __result = commands;
        }
    }

    [Preserve]
    [HarmonyPatch(typeof(Entity), nameof(Entity.OnEntityActivated))]
    internal static class CorpseCommandActivationPatch
    {
        [Preserve]
        private static bool Prefix(Entity __instance, EntityActivationCommand _command)
        {
            if (!CorpseInteraction.IsSupported(__instance)
                || !CorpseInteraction.IsSearchCommand(_command.commandId))
            {
                return true;
            }

            if (CorpseInteraction.CanSearch(__instance))
            {
                CorpseInteraction.RequestLock(
                    __instance,
                    new Entity.EntityLockContext(_command.commandId, __instance.bag));
            }

            return false;
        }
    }

    [Preserve]
    [HarmonyPatch(typeof(Entity), nameof(Entity.CanLockOnServer))]
    internal static class CorpseServerLockPatch
    {
        [Preserve]
        private static void Postfix(
            Entity __instance,
            int _lockingPlayerID,
            ILockContext _context,
            ref bool __result)
        {
            if (__result
                || __instance.bag != null
                || !CorpseInteraction.IsSupported(__instance)
                || !__instance.IsDead()
                || !CorpseInteraction.IsSearchContext(_context))
            {
                return;
            }

            if (__instance.spawnById > 0
                && __instance.spawnById != _lockingPlayerID
                && !__instance.spawnByAllowShare)
            {
                return;
            }

            __result = CorpseInteraction.EnsureBag(__instance);
        }
    }

    [Preserve]
    [HarmonyPatch(typeof(Entity), nameof(Entity.CanLockLocally))]
    internal static class CorpseLocalLockPatch
    {
        [Preserve]
        private static void Postfix(Entity __instance, ILockContext _context, ref bool __result)
        {
            if (__result
                || !CorpseInteraction.IsSupported(__instance)
                || !__instance.IsDead()
                || !CorpseInteraction.IsSearchContext(_context))
            {
                return;
            }

            if (CorpseInteraction.EnsureBag(__instance))
            {
                __result = LocalPlayerUI.GetUIForPrimaryPlayer() != null;
            }
        }
    }

    [Preserve]
    [HarmonyPatch(typeof(Entity), nameof(Entity.OnLockedServer))]
    internal static class CorpseServerLockedPatch
    {
        [Preserve]
        private static void Prefix(Entity __instance, bool _success, ILockContext _context)
        {
            if (_success
                && CorpseInteraction.IsSupported(__instance)
                && __instance.IsDead()
                && CorpseInteraction.IsSearchContext(_context))
            {
                CorpseInteraction.EnsureBag(__instance);
            }
        }
    }

    [Preserve]
    [HarmonyPatch(typeof(Entity), nameof(Entity.GetActivationText))]
    internal static class CorpseActivationTextPatch
    {
        [Preserve]
        private static void Postfix(Entity __instance, ref string __result)
        {
            if (!CorpseInteraction.CanSearch(__instance))
            {
                return;
            }

            EntityPlayerLocal player = GameManager.Instance?.World?.GetPrimaryPlayer();
            if (player == null)
            {
                return;
            }

            string binding = player.playerInput.Activate.GetBindingXuiMarkupString()
                + player.playerInput.PermanentActions.Activate.GetBindingXuiMarkupString();
            string name = Localization.Get(__instance.LocalizedEntityName);

            if (!__instance.bag.Touched)
            {
                __result = string.Format(Localization.Get("lootTooltipNew"), binding, name);
            }
            else if (__instance.bag.IsEmpty())
            {
                __result = string.Format(Localization.Get("lootTooltipEmpty"), binding, name);
            }
            else
            {
                __result = string.Format(Localization.Get("lootTooltipTouched"), binding, name);
            }
        }
    }
}
