using System;
using Match;
using Score.View;
using Services;
using Signals;
using Targets;
using Targets.Enums;
using Targets.Managers;
using UniRx;
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
        
        [SerializeField] private ScoreView _scoreView;
        private MatchData _currentMatchData;
        private IntReactiveProperty _currentScore;

        private void Awake()
        {
            _currentScore = new IntReactiveProperty();
            _scoreView.LinkReactProperty(_currentScore);
            
            _signalBus.Subscribe<MatchStartedSignal>(Initialize);
            _signalBus.Subscribe<ResetMatchSignal>(ResetController);
            _signalBus.Subscribe<FinishZoneHealthEmptySignal>(OnMatchCompleted);
        }
        
        private void Initialize(MatchStartedSignal signal)
        {
            _currentMatchData = signal.MatchData;
            
            _currentScore.Value = 0;
            _targetsController.TargetKilled += OnTargetKilled;
        }
        
        private void OnTargetKilled(Target target)
        {
            switch (target.Variant)
            {
                case TargetVariant.ENEMY:
                    _currentScore.Value++;
                    break;
                case TargetVariant.ALLY: //move to config "score per ..."?
                    _currentScore.Value--;
                    break;
            }

            _currentScore.Value = Mathf.Clamp(_currentScore.Value, 0, int.MaxValue);
            CurrentScoreChanged?.Invoke(_currentScore.Value);
        }
        
        private void OnMatchCompleted()
        {
            _servicesHolder.GetService<ProgressService>().TryUpdateRecord(_currentScore.Value);
            _signalBus.Fire(new MatchCompletedSignal(_currentScore.Value));
        }
        
        private void ResetController()
        {
            _currentScore.Value = 0;
            _targetsController.TargetKilled -= OnTargetKilled;
        }
    }
}
