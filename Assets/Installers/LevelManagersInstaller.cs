using Levels.Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelManagersInstaller : MonoInstaller
    {
        [SerializeField] private LevelBootstrapper _levelBootstrapper;

        public override void InstallBindings()
        {
            Container.BindInstance(_levelBootstrapper).AsSingle().NonLazy();
        }
    }
}
