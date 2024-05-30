using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BootstrapperInstaller : MonoInstaller
    {
        [SerializeField] private Bootstrapper _bootstrapper;
        
        public override void InstallBindings()
        {
            Container.Bind<Bootstrapper>().FromComponentInNewPrefab(_bootstrapper).AsSingle().NonLazy();
        }
    }
}
