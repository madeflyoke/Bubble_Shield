using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ManagersInstaller : MonoInstaller
    {
        [SerializeField] private Bootstrapper _bootstrapper;
        
        public override void InstallBindings()
        {
            Container.Bind<Bootstrapper>().FromComponentInNewPrefab(_bootstrapper).AsSingle().NonLazy();
        }
    }
}
