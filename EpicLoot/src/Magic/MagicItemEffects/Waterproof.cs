﻿using HarmonyLib;

namespace EpicLoot.MagicItemEffects
{
    public static class Waterproof
    {
        public static int AddingStatusFromEnv;

        [HarmonyPatch(typeof(Player), nameof(Player.UpdateEnvStatusEffects))]
        public static class Waterproof_Player_UpdateEnvStatusEffects_Patch
        {
            public static bool Prefix()
            {
                AddingStatusFromEnv++;
                return true;
            }

            public static void Postfix(Player __instance)
            {
                AddingStatusFromEnv--;
            }
        }

        [HarmonyPatch(typeof(SEMan), nameof(SEMan.AddStatusEffect), typeof(int), typeof(bool), typeof(int), typeof(float))]
        public static class Waterproof_SEMan_AddStatusEffect_Patch
        {
            public static bool Prefix(SEMan __instance, int nameHash)
            {
                if (AddingStatusFromEnv > 0 && __instance.m_character.IsPlayer() && nameHash == "Wet".GetHashCode())
                {
                    var player = (Player) __instance.m_character;
                    var hasWaterproofEquipment = player.HasActiveMagicEffect(MagicEffectType.Waterproof, out float effectValue);
                    if (hasWaterproofEquipment)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
    
}
