using System;
using System.Collections.Generic;
using System.Threading;
using Managers;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class ServicesHolder : IDisposable
    {
        private Dictionary<Type,IService> _services;
        private CancellationTokenSource _cts;
        private bool _isInitialized;
        
        public async void InitializeServices(Action onInitialized)
        {
            if (_isInitialized)
            {
                return;
            }
            
            TService AddService<TService>() where TService: IService
            {
                var instance = Activator.CreateInstance<TService>();
                _services.Add(typeof(TService), instance);
                return instance;
            }
            
            _services = new Dictionary<Type, IService>();
            //add all services below
            AddService<YandexService>(); 
            AddService<PauseService>();
            
            _cts = new CancellationTokenSource();

            foreach (var service in _services)
            {
                Debug.LogWarning($"Service {service.Value} started initialization...");
                await service.Value.Initialize(_cts);
                Debug.LogWarning($"Service {service.Value} initialized2");
            }
            
            onInitialized?.Invoke();
            _isInitialized = true;
        }
        
        public TService GetService<TService>() where TService: IService
        {
            return (TService) _services[typeof(TService)];
        }

        public void Dispose()
        {
            _cts?.Cancel();
        }
    }
}
