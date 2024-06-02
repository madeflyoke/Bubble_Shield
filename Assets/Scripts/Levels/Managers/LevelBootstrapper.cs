using Levels.Configs;
using Signals;
using UnityEngine;
using Zenject;

namespace Levels.Managers
{
    public class LevelBootstrapper : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        public LevelData CurrentLevelData { get; private set; }
        
        [SerializeField] private LevelsConfig _levelsConfig;

        public void Start()
        {
            _signalBus.Subscribe<LevelSelectedSignal>(OnLevelSelected);
            _signalBus.Subscribe<CallOnRestartLevel>(RestartCurrentLevel);
        }

        private void OnLevelSelected(LevelSelectedSignal signal)
        {
            var levelData = _levelsConfig.GetLevelData(signal.LevelId);
            CurrentLevelData = levelData;
            _signalBus.Fire(new LevelStartedSignal(levelData));
        }

        private void RestartCurrentLevel()
        {
            _signalBus.Fire(new LevelStartedSignal(CurrentLevelData));
        }
    }
}
