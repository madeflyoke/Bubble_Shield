using Match.Configs;
using Signals;
using UnityEngine;
using Zenject;

namespace Match.Managers
{
    public class MatchBootstrapper : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private MatchConfig _matchConfig;
        
        private void Awake()
        {
            _signalBus.Subscribe<StartMatchCallSignal>(StartMatch);
        }
        

        private void StartMatch()
        {
            _signalBus.Fire(new MatchStartedSignal(_matchConfig.MatchData));
        }
    }
}
