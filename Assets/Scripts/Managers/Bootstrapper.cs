using Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Managers
{
    public class Bootstrapper : MonoBehaviour
    {
        [Inject] private ServicesHolder _servicesHolder;
        
        [SerializeField] private string _mainSceneName;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;
            _servicesHolder.InitializeServices(LoadMainScene);
        }
        
        private void LoadMainScene()
        {
            SceneManager.LoadSceneAsync(_mainSceneName);
        }

        private void OnDestroy()
        {
            _servicesHolder?.Dispose();
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
