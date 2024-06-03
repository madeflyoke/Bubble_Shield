using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public static class AutoPlayModeScene
    {
#if UNITY_EDITOR
        
        private const string SceneAssetPath = "Assets/Scenes/Bootstrapper.unity";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SetFirstSceneAsActive()
        {
            Debug.LogWarning("Set initial scene...");
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(SceneAssetPath);
            if (sceneAsset != null)
            {
                var scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                SceneManager.LoadScene(scenePath);
            }
        }
        
#endif
    }
}