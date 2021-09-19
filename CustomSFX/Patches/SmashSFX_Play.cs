using CustomSFX.Extensions;
using CustomSFX.Managers;
using CustomSFX.Utilities;
using HarmonyLib;
using Smash;
using System;
using System.Reflection;
using UnityEngine;

namespace CustomSFX.Patches
{
    [HarmonyPatch(typeof(SmashSFX), "Play", new Type[] { typeof(AudioSource) })]
    class SmashSFX_Play
    {
        static readonly MethodInfo SmashSFX_Blocked = typeof(SmashSFX).GetMethod("Blocked", BindingFlags.Static | BindingFlags.NonPublic);
        static readonly MethodInfo SmashSFX_AddBlock = typeof(SmashSFX).GetMethod("AddBlock", BindingFlags.Static | BindingFlags.NonPublic);

        static bool Prefix(SmashSFX __instance, AudioSource staticSource)
        {
            var id = __instance.gameObject.name.ToLower();

            if (SFXManager.customSFXDict.TryGetValue("global", out var bank))
            {
                if (bank.TryGetValue(id, out var list))
                {
                    if (__instance.blockRepeatPlayback)
                    {
                        if ((bool)SmashSFX_Blocked.Invoke(obj: null, parameters: new object[] { __instance.blockRepeatPlaybackId }))
                        {
                            return false;
                        }
                        SmashSFX_AddBlock.Invoke(obj: null, parameters: new object[] { __instance.blockRepeatPlaybackId, __instance.blockRepeatPlayback_ms * 0.001f });
                    }
                    if (__instance.sfx.Length == 0)
                    {
                        return false;
                    }

                    AudioClip clip;
                    if (list.Count == 0) return false;
                    if (list.Count == 1)
                        clip = list[0];
                    else
                        clip = list.GetRandomElement();

                    SmashSFX.SFXLikely randomClip = __instance.InvokeMethod<SmashSFX.SFXLikely>("GetRandomClip");
                    SmashSound.Instance.PlaySFX(clip, randomClip.volume, __instance.sidechain, randomClip.Pitch, staticSource, null, __instance.delay);
                    return false;
                }
            }

            return true;
        }
    }
}
