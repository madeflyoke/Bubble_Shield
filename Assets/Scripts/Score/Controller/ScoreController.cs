using Managers.Targets;
using Score.Model;
using Score.View;
using Targets;
using Targets.Enums;
using UnityEngine;

namespace Score.Controller
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private TargetsController _targetsController;

        [SerializeField] private ScoreView _scoreView;
        private ScoreData _scoreData;
        private int _currentScore;

        private void Awake()
        {
            _scoreData = new ScoreData();
            _scoreView.SetScoreText(0);
            _scoreView.SetRecordScoreText(_scoreData.RecordScoreValue);
        }

        private void OnEnable()
        {
            _targetsController.TargetFinished += OnTargetFinished;
        }
        private void OnDisable()
        {
            _targetsController.TargetFinished -= OnTargetFinished;
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
            _scoreView.SetScoreText(_currentScore);
            
            if (_currentScore>_scoreData.RecordScoreValue)
            {
                _scoreData.SetRecordScoreValue(_currentScore,true);
                _scoreView.SetRecordScoreText(_currentScore);
            }
        }
    }
}
