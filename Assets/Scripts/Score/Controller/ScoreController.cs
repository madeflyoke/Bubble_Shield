using System;
using Match;
using Score.View;
using Services;
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
        [Inject] private TargetsController _targetsController;
        [Inject] private ServicesHolder _servicesHolder;

        public event Action<int> CurrentScoreChanged;
        private int _currentScore;
        
        [SerializeField] private ScoreView _scoreView;
        private MatchData _currentMatchData;

        private void Awake()
        {
            _signalBus.Subscribe<MatchStartedSignal>(Initialize);
            _signalBus.Subscribe<ResetMatchSignal>(ResetController);
            _signalBus.Subscribe<MatchCompletedSignal>(OnMatchCompleted);
        }
        
        private void Initialize(MatchStartedSignal signal)
        {
            _currentMatchData = signal.MatchData;
            
            _currentScore = 0;
            _scoreView.SetCurrentScore(_currentScore);
            _targetsController.TargetKilled += OnTargetKilled;
        }
        
        private void OnTargetKilled(Target target)
        {
            switch (target.Variant)
            {
                case TargetVariant.ENEMY:
                    _currentScore++;
                    break;
                case TargetVariant.ALLY: //move to config "score per ..."?
                    _currentScore--;
                    break;
            }

            _currentScore = Mathf.Clamp(_currentScore, 0, int.MaxValue);
            _scoreView.SetCurrentScore(_currentScore);

            CurrentScoreChanged?.Invoke(_currentScore);
        }
        
        private void OnMatchCompleted()
        {
            _servicesHolder.GetService<ProgressService>().TryUpdateRecord(_currentScore);
        }
        
        private void ResetController()
        {
            _currentScore = 0;
            _scoreView.SetCurrentScore(_currentScore);
        }
    }
}
