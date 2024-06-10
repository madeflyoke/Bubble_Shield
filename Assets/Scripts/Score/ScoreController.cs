using Match;
using Services;
using Signals;
using Targets;
using Targets.Enums;
using Targets.Managers;
using UniRx;
using UnityEngine;
using Zenject;

namespace Score
{
    public class ScoreController : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private TargetsController _targetsController;
        [Inject] private ServicesHolder _servicesHolder;
        
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private ScoreCombo _scoreCombo;
        
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
            if (target.Variant == TargetVariant.ENEMY)
            {
                _scoreCombo.TrySetCombo(true);
                _currentScore.Value += _scoreCombo.CurrentCombo;
                _currentScore.Value = Mathf.Clamp(_currentScore.Value, 0, int.MaxValue);
            }
            else
            {
                _scoreCombo.TrySetCombo(false);
            }
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
            _scoreCombo.ResetCombo();
        }
    }
}
