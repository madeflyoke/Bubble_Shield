using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using EasyButtons;
using Services;
using Services.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private string _mainSceneName;
        private Dictionary<Type,IService> _services;
        private CancellationTokenSource _cts;
        
        private void Awake()
        {
            
        }

        private async void InitializeServices()
        {
            _services = new Dictionary<Type, IService>();
            AddService<YandexService>();
            
            
            foreach (var service in _services)
            {
                await UniTask. service.Value.Initialize();
            }
            
            TService AddService<TService>() where TService: IService
            {
                var instance = Activator.CreateInstance<TService>();
                _services.Add(typeof(TService), instance);
                return instance;
            }
        }

        private void Start()
        {
            LoadMainScene();
        }

        [Button]
        private void LoadMainScene()
        {
            SceneManager.LoadSceneAsync(_mainSceneName);
        }

        private TService GetService<TService>() where TService: IService
        {
            return (TService) _services[typeof(TService)];
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
