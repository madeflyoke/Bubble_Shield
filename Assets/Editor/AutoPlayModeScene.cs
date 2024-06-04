#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor
{
    [InitializeOnLoad]
    public class AutoPlayModeScene
    {
        static AutoPlayModeScene()
        {
            var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
            EditorSceneManager.playModeStartScene = sceneAsset;
        }
    }
}

#endif