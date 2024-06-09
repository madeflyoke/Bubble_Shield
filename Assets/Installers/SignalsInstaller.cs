using Signals;
using Zenject;

namespace Installers
{
    public class SignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<MatchStartedSignal>();
            Container.DeclareSignal<MatchCompletedSignal>();
            Container.DeclareSignal<FinishZoneHealthEmptySignal>();
            Container.DeclareSignal<StartMatchCallSignal>();
            Container.DeclareSignal<ResetMatchSignal>();
        }
    }
}
