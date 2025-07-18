using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LookingGlass
{
    public class LookingGlassManager:SingletonMono<LookingGlassManager>
    {
        private Camera MainCamera;
        private Camera ScreenCamera;
        private Dictionary<string, LookingGlassSceneData> registeredScenes;
        private LookingGlassSceneData screenScene;
        private LookingGlassSceneData currentScene;
        private LookingGlassController Controller;
        private Transform spawnTransform;
        private LookingGlassSceneLoader Loader;

        protected override void Awake()
        {
           setup();
        }

        public void setup()
        {
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
            if (MainCamera == null)
            {
                Debug.LogError("LookingGlassManager: MainCamera not found");
            }
            ScreenCamera = GameObject.FindGameObjectWithTag("ScreenCamera")?.GetComponent<Camera>();
            if (ScreenCamera == null)
            {
                Debug.LogError("LookingGlassManager: ScreenCamera not found");
            }
            
            
        }
        public static RenderTexture  GetScreenRT()
        {
            return Instance.ScreenCamera.activeTexture;
        }
        public static void RegisterScene(string name, LookingGlassSceneData metaData)
        {
            Instance.registeredScenes.Add(name, metaData);

            if (Instance.currentScene == null) 
            {
                Instance.currentScene = metaData;
            }
        }
        
        public static void EnableScene(LookingGlassSceneLoader sceneLoader)
        {
            LookingGlassSceneData sceneMetaData = Instance.registeredScenes[sceneLoader.SceneName];

            if (sceneMetaData == null)
            {
                throw new Exception("Trying to enable unregistered scene");
            }

            Debug.Log("Enabling this scene: " + sceneMetaData.Scene.name);

            Instance.Loader = sceneLoader;
            Instance.screenScene = sceneMetaData;

            LightProbes.TetrahedralizeAsync();

            
            sceneMetaData.root.SetActive(true);
            

      
            if (sceneMetaData.SpawnTransform != null)
            {
                Instance.ScreenCamera.GetComponent<VirtualCamera>().SetOffset(
                    sceneMetaData.SpawnTransform.position - Instance.Loader.ReferencePoint.position);
            }


            if (sceneLoader.screenController != null)
            {
                sceneLoader.screenController.enableLookingGlass();
            }
        
          
            int index = sceneMetaData.RendererIndex > 1 ? sceneMetaData.RendererIndex : 1;
            Instance.ScreenCamera.GetComponent<UniversalAdditionalCameraData>().SetRenderer(index);
        
            Instance.ScreenCamera.GetComponent<Camera>().enabled = true;
            
        }

        public static void DisableScene(LookingGlassSceneLoader sceneLoader){}
    }
}