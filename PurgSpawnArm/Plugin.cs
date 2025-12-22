using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;


namespace PurgSpawnArm
{

    [HarmonyPatch]
    [BepInPlugin(GUID, "Purg Spawn Arm", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        const string GUID = "purg.PurgSpawnArm"; //COPIED CODE, REPLACE STRING

        static Harmony _harmony;

        static SpawnableObject[] spawnableObjects;

        static AssetBundle bundle;
        static AssetBundle shadersBundle;

        static List<Shader> loadedShaders;
        static List<AudioSource> loadedAudioSources;
        
        static bool doneTheBullshit = false;
        
        public const string bundleName = "purg-indulgenceenemies"; // replace with name of bundle

        void Start()
        {
            
            foreach (var loadedAssetBundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                Debug.Log(loadedAssetBundle.name);
                if (loadedAssetBundle.name == "shaders.bundle")
                {
                    shadersBundle = loadedAssetBundle;
                }
            }

            if (!shadersBundle)
            {
                string correctPath = Path.Combine(Application.streamingAssetsPath, "aa/StandaloneWindows64/assets_assets_assets/shaders.bundle");
                shadersBundle = AssetBundle.LoadFromFile(correctPath);
            }
            
            //load bundle
            bundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Info.Location), bundleName));
            spawnableObjects = bundle.LoadAllAssets<SpawnableObject>();




            //loadedShaders = shadersBundle.LoadAllAssets<Shader>().ToList();
            
            //foreach (var loadedShader in loadedShaders)
            //{
            //9847c6449c4ab6345a943c38df306a85
            var fuckingTHing = Addressables.LoadAssetAsync<Shader>("9847c6449c4ab6345a943c38df306a85").Result;
            foreach (var mat in bundle.LoadAllAssets<Material>())
            {
                if (mat.shader.name == "ULTRAKILL/Master")
                {
                    SwapShader(mat, fuckingTHing);
                }
            }
            //}
            
            
            loadedAudioSources = bundle.LoadAllAssets<AudioSource>().ToList();
            
            _harmony = new Harmony(GUID);
            _harmony.PatchAll();
        }
        
                
        static void SwapShader(Material material, Shader shader) 
        {
            int renderQueue = material.renderQueue;
            material.shader = shader;
            material.renderQueue = renderQueue;
        }

        
        [HarmonyPatch(typeof(AudioMixerController), "Start")]
        [HarmonyPrefix]
        private static void AddCheats(AudioMixerController __instance)
        {
            if (!doneTheBullshit)
                return;
            
            foreach (var loadAllAsset in loadedAudioSources)
            {
                switch (loadAllAsset.outputAudioMixerGroup.name)
                {
                    case "AllAudio":
                        loadAllAsset.outputAudioMixerGroup = __instance.allGroup;
                        break;
                    case "GoreAudio":
                        loadAllAsset.outputAudioMixerGroup = __instance.goreGroup;
                        break;
                    case "MusicAudio":
                        loadAllAsset.outputAudioMixerGroup = __instance.musicGroup;
                        break;
                    case "DoorAudio":
                        loadAllAsset.outputAudioMixerGroup = __instance.doorGroup;
                        break;
                    case "UnfreezeableAudio":
                        loadAllAsset.outputAudioMixerGroup = __instance.unfreezeableGroup;
                        break;
                }
            }

            doneTheBullshit = true;
        }

        

        [HarmonyPatch(typeof(SpawnMenu), "RebuildMenu"), HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        private static void RebuildMenu(SpawnMenu __instance)
        {
            if (SceneHelper.IsPlayingCustom)
                return;
            
            var enemiesList = __instance.objects.enemies.ToList();
            var objectsList = __instance.objects.objects.ToList();
            foreach (var spawnable in spawnableObjects)
            {
                spawnable.sandboxOnly = false;
                if (spawnable.spawnableObjectType == SpawnableObject.SpawnableObjectDataType.Enemy && !__instance.objects.enemies.Contains(spawnable))
                {
                    enemiesList.Add(spawnable);
                }

                if (spawnable.spawnableObjectType == SpawnableObject.SpawnableObjectDataType.Object && !__instance.objects.objects.Contains(spawnable))
                {
                    objectsList.Add(spawnable);
                }
            }
            __instance.objects.enemies = enemiesList.ToArray();
            __instance.objects.objects = objectsList.ToArray();
        }
    }
}
