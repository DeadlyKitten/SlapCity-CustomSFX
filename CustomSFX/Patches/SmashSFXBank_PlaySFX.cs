using CustomSFX.Extensions;
using CustomSFX.Managers;
using HarmonyLib;
using CustomSFX.Utilities;
using Smash;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSFX.Patches
{
    [HarmonyPatch(typeof(SmashSFXBank), "PlaySFX")]
    class SmashSFXBank_PlaySFX
    {
        static bool Prefix(SmashSFXBank __instance, string id, SmashCharacter sc)
        {
            var key = (sc == null) ? "global": sc.gameObject.name.Split('(')[0].ToLower();
            if (SFXManager.customSFXDict.TryGetValue(key, out var bank))
            {
                if (bank.TryGetValue(id, out var sfx))
                    return TryPlaySFX(__instance, id, sc, sfx);
            }
            if (key != "global" && SFXManager.customSFXDict.TryGetValue("global", out bank))
            {
                if (bank.TryGetValue(id, out var sfx))
                    return TryPlaySFX(__instance, id, sc, sfx);
            }

            return true;
        }

        private static bool TryPlaySFX(SmashSFXBank __instance, string id, SmashCharacter sc, List<AudioClip> sfx)
        {
            var ED = __instance.GetProperty<Dictionary<string, SmashSFX>>("ED");
            if (!ED.ContainsKey(id)) return false;

            var smashSFX = ED[id];
            smashSFX.Play(sfx, sc);
            return false;
        }
    }
}
