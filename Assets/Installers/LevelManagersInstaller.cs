using Levels.Managers;
using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelManagersInstaller : MonoInstaller
    {
        [SerializeField] private LevelBootstrapper _levelBootstrapper;
        [SerializeField] private PauseManager _pauseManager;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_levelBootstrapper).AsSingle().NonLazy();
            Container.BindInstance(_pauseManager).AsSingle().NonLazy();
        }
    }
}
