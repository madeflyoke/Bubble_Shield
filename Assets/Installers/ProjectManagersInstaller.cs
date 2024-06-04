using Managers;
using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ProjectManagersInstaller : MonoInstaller
    {
        [SerializeField] private Bootstrapper _bootstrapper;
        
        public override void InstallBindings()
        {
            Container.Bind<Bootstrapper>().FromComponentInNewPrefab(_bootstrapper).AsSingle().NonLazy();
            Container.BindInstance(new ServicesHolder(Container)).AsSingle().NonLazy();
        }
    }
}
