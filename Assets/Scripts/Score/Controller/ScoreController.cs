using System;
using Score.View;
using Signals;
using Targets;
using Targets.Enums;
using Targets.Managers;
using UnityEngine;
using Zenject;

namespace Score.Controller
{
    public class ScoreController : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private TargetsController _targetsController;
        [SerializeField] private ScoreView _scoreView;
        private int _currentScore;
        private int _targetScore;

        private void Start()
        {
            _signalBus.Subscribe<LevelStartedSignal>(Initialize);
        }

        private void Initialize(LevelStartedSignal signal)
        {         
            var targetScore = signal.LevelData.TargetScore;
            
            _currentScore = 0;
            _targetScore =targetScore;
            
            _scoreView.SetCurrentScore(_currentScore);
            _scoreView.SetTargetScore(_targetScore);
            
            _targetsController.TargetFinished += OnTargetFinished;
        }
        
        private void OnTargetFinished(Target target)
        {
            switch (target.Variant)
            {
                case TargetVariant.ENEMY:
                    _currentScore--; 
                    break;
                case TargetVariant.ALLY: //move to config "score per ..."?
                    _currentScore++;
                    break;
            }

            _currentScore = Mathf.Clamp(_currentScore, 0, int.MaxValue);
            _scoreView.SetCurrentScore(_currentScore);
            
            if (_currentScore==_targetScore)
            {
                _signalBus.Fire<LevelCompletedSignal>();
                _targetsController.TargetFinished -= OnTargetFinished;
            }
        }
    }
}
