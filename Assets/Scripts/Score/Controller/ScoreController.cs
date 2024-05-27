using System;
using Levels;
using Score.View;
using Targets;
using Targets.Enums;
using Targets.Managers;
using UnityEngine;

namespace Score.Controller
{
    public class ScoreController : MonoBehaviour
    {
        public event Action TargetScoreReached; 
        
        [SerializeField] private TargetsController _targetsController;

        [SerializeField] private ScoreView _scoreView;
        private int _currentScore;
        private int _targetScore;

        public void Initialize(int targetScore)
        {
            _scoreView.SetCurrentScore(0);
            _scoreView.SetTargetScore(targetScore);
            _targetScore =targetScore;
            
            _targetsController.TargetFinished += OnTargetFinished;
        }
        
        private void OnTargetFinished(Target target)
        {
            Debug.LogWarning(target.Variant);
            switch (target.Variant)
            {
                case TargetVariant.ENEMY:
                    _currentScore--; 
                    break;
                case TargetVariant.ALLY: //1 move to config "score per ..."?
                    _currentScore++;
                    break;
            }

            _currentScore = Mathf.Clamp(_currentScore, 0, int.MaxValue);
            _scoreView.SetCurrentScore(_currentScore);
            
            if (_currentScore==_targetScore)
            {
                TargetScoreReached?.Invoke();
                _targetsController.TargetFinished -= OnTargetFinished;
            }
        }
    }
}
