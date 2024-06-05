using Levels.Managers;
using UI.Screens;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelManagersInstaller : MonoInstaller
    {
        [SerializeField] private LevelBootstrapper _levelBootstrapper;
        [SerializeField] private ScreensController _screensController;

        public override void InstallBindings()
        {
            Container.BindInstance(_levelBootstrapper).AsSingle().NonLazy();
            Container.BindInstance(_screensController).AsSingle().NonLazy();
        }
    }
}
