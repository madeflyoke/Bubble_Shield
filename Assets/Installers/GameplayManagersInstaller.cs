using Match.Managers;
using Targets.Managers;
using UI.Screens;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameplayManagersInstaller : MonoInstaller
    {
        [SerializeField] private MatchBootstrapper _matchBootstrapper;
        [SerializeField] private ScreensController _screensController;
        [SerializeField] private TargetsController _targetsController;

        public override void InstallBindings()
        {
            Container.BindInstance(_matchBootstrapper).AsSingle().NonLazy();
            Container.BindInstance(_screensController).AsSingle().NonLazy();
            Container.BindInstance(_targetsController).AsSingle().NonLazy();
        }
    }
}
