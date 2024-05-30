using System;
using Levels.Configs;
using Score.Controller;
using Signals;
using Targets.Managers;
using UI.LevelSelection;
using UnityEngine;
using Zenject;

namespace Levels.Managers
{
    public class LevelBootstrapper : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private LevelsConfig _levelsConfig;

        public void Start()
        {
            _signalBus.Subscribe<LevelSelectedSignal>(OnLevelSelected);
        }

        private void OnLevelSelected(LevelSelectedSignal signal)
        {
            var levelData = _levelsConfig.GetLevelData(signal.LevelId);
            _signalBus.Fire(new LevelStartedSignal(levelData));
        }
    }
}
