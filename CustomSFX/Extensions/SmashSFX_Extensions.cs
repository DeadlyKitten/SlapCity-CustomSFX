using CustomSFX.Utilities;
using Smash;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

namespace CustomSFX.Extensions
{
    static class SmashSFX_Extensions
    {
		static readonly MethodInfo SmashSFX_Blocked = typeof(SmashSFX).GetMethod("Blocked", BindingFlags.Static | BindingFlags.NonPublic);
		static readonly MethodInfo SmashSFX_AddBlock = typeof(SmashSFX).GetMethod("AddBlock", BindingFlags.Static | BindingFlags.NonPublic);

        public static void Play(this SmashSFX sfx, List<AudioClip> clips, object obj = null)
        {
			if (sfx.blockRepeatPlayback)
			{
				if ((bool) SmashSFX_Blocked.Invoke(obj: null, parameters: new object[] { sfx.blockRepeatPlaybackId }))
				{
					return;
				}
				SmashSFX_AddBlock.Invoke(obj: null, parameters: new object[] { sfx.blockRepeatPlaybackId, sfx.blockRepeatPlayback_ms * 0.001f });
			}
			if (sfx.sfx.Length == 0)
			{
				return;
			}

			AudioClip clip;
			if (clips.Count == 0) return;
			if (clips.Count == 1)
				clip = clips[0];
			else
				clip = clips.GetRandomElement();

			var randomClip = sfx.InvokeMethod<SmashSFX.SFXLikely>("GetRandomClip");
			SmashSound.Instance.PlaySFX(clip, randomClip.volume, sfx.sidechain, randomClip.Pitch, null, (!sfx.isInterruptable) ? null : obj, sfx.delay);
			if (sfx.layeredSound != null)
			{
				sfx.layeredSound.Play(null);
			}
		}

		public static T GetRandomElement<T>(this List<T> list)
        {
			return list[Random.Range(0, list.Count)];
        }
    }
}
