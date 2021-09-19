using BepInEx;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace CustomSFX.Managers
{
    class SFXManager
    {
        internal static Dictionary<string, Dictionary<string, List<AudioClip>>> customSFXDict;

        private static int stillLoading = 0;
        private static int sfxCount = 0;

        internal static void Init()
        {
            customSFXDict = new Dictionary<string, Dictionary<string, List<AudioClip>>>();
            LoadSFX();
        }

        static void LoadSFX()
        {
            Plugin.LogDebug("Loading custom sfx...");
            var watch = new Stopwatch();
            watch.Start();

            var mainFolderPath = Path.Combine(Paths.BepInExRootPath, "Custom SFX");
            Directory.CreateDirectory(mainFolderPath);

            foreach (var character in characters)
            {
                var characterPath = Path.Combine(mainFolderPath, character);
                Directory.CreateDirectory(characterPath);

                WriteFile(characterPath);

                foreach (var file in Directory.GetFiles(characterPath).Where(x => Path.GetExtension(x) == ".ogg" || Path.GetExtension(x) == ".wav"))
                {
                    Plugin.Instance.StartCoroutine(LoadAudioClip(file));
                    stillLoading++;
                }

                foreach (var directory in Directory.GetDirectories(characterPath))
                {
                    foreach (var file in Directory.GetFiles(directory).Where(x => Path.GetExtension(x) == ".ogg" || Path.GetExtension(x) == ".wav"))
                    {
                        var _character = Directory.GetParent(directory).Name;
                        var _name = Directory.GetParent(file).Name;

                        Plugin.Instance.StartCoroutine(LoadAudioClip(file, _character, _name));
                        stillLoading++;
                    }
                }
            }

            Plugin.Instance.StartCoroutine(WaitForLoadComplete(watch));
        }

        static IEnumerator LoadAudioClip(string path, string character = null, string name = null)
        {
            var audioType = (Path.GetExtension(path) == ".wav") ? AudioType.WAV : AudioType.OGGVORBIS;
            var loader = UnityWebRequestMultimedia.GetAudioClip(path, audioType);
            yield return loader.SendWebRequest();

            if (loader.error != null)
            {
                Plugin.LogError($"Error loading song from path: {path}\n{loader.error}");
                stillLoading--;
                yield break;
            }

            var clip = DownloadHandlerAudioClip.GetContent(loader);
            character ??= Directory.GetParent(path).Name;
            name ??= Path.GetFileNameWithoutExtension(path);

            if (customSFXDict.TryGetValue(character, out var dict))
            {
                if (dict.TryGetValue(name, out var list))
                    list.Add(clip);
                else
                    dict.Add(name, new List<AudioClip>() { clip });
            }
            else
            {
                var sfx = new Dictionary<string, List<AudioClip>>() { { name, new List<AudioClip>() { clip } } };
                customSFXDict.Add(character, sfx);
            }

            sfxCount++;
            stillLoading--;
        }

        static IEnumerator WaitForLoadComplete(Stopwatch watch)
        {
            yield return new WaitUntil(() => stillLoading == 0);
            watch.Stop();
            Plugin.LogInfo($"Loaded {sfxCount} custom sound effects in {watch.ElapsedMilliseconds} ms.");
        }

        static async void WriteFile(string path)
        {
            string textToWrite;

            var assembly = Assembly.GetExecutingAssembly();
            var filename = $"{path.Split('\\').Last()}.txt";
            var resourceName = $"CustomSFX.Resources.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return;
                using StreamReader reader = new StreamReader(stream);
                textToWrite = reader.ReadToEnd();
            }

            if (textToWrite == null) return;

            using var writer = new StreamWriter(Path.Combine(path, filename));
            await writer.WriteAsync(textToWrite);
        }

        static readonly string[] characters =
        {
            "global",
            "asha",
            "bcman",
            "ruby",
            "ittledew",
            "frallan",
            "goddess",
            "remedy",
            "jennyfox",
            "fishbunjin"
        };
    }
}
