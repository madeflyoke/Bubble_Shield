using System;
using System.Collections.Generic;
using System.Threading;
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
            InitializeServices();
        }

        private async void InitializeServices()
        {
            TService AddService<TService>() where TService: IService
            {
                var instance = Activator.CreateInstance<TService>();
                _services.Add(typeof(TService), instance);
                return instance;
            }
            
            _services = new Dictionary<Type, IService>();
            AddService<YandexService>();
            
            _cts = new CancellationTokenSource();

            foreach (var service in _services)
            {
                Debug.LogWarning($"Service {service.Value} started initialization...");
                await service.Value.Initialize(_cts);
                Debug.LogWarning($"Service {service.Value} initialized");
            }
            
            LoadMainScene();
        }
        
        private void LoadMainScene()
        {
            SceneManager.LoadSceneAsync(_mainSceneName);
        }

        private TService GetService<TService>() where TService: IService
        {
            return (TService) _services[typeof(TService)];
        }
        
        private void OnDestroy()
        {
            _cts?.Cancel();
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
