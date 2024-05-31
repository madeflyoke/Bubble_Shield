using Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class Bootstrapper : MonoBehaviour
    {
        public ServicesHolder ServicesHolder { get; private set; }

        [SerializeField] private string _mainSceneName;

        private void Awake()
        {
            ServicesHolder = new ServicesHolder();
            ServicesHolder.InitializeServices(LoadMainScene);
        }
        
        private void LoadMainScene()
        {
            SceneManager.LoadSceneAsync(_mainSceneName);
        }

        private void OnDestroy()
        {
            ServicesHolder?.Dispose();
        }


#if UNITY_EDITOR

        [SerializeField] private SceneAsset EDITOR_mainScene;

        private void OnValidate()
        {
            _mainSceneName = EDITOR_mainScene.name;
        }

#endif
    }
}
