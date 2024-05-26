using Managers;
using Score.Model;
using Targets;
using Targets.Enums;
using UI;
using UnityEngine;

namespace Score.Controller
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private TargetsController _targetsController;

        [SerializeField] private ScoreView _scoreView;
        private ScoreData _scoreData;

        private void Awake()
        {
            _scoreData = new ScoreData();
            _scoreView.SetScoreText(_scoreData.CurrentScoreValue);
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
            int newScore = 0;
            switch (target.Variant)
            {
                case TargetVariant.ENEMY:
                    newScore = _scoreData.CurrentScoreValue+1; //1 move to config "score per enemy"?
                    break;
                case TargetVariant.ALLY:
                    newScore = _scoreData.CurrentScoreValue-1;
                    break;
            }

            newScore = Mathf.Clamp(newScore, 0, int.MaxValue);
            _scoreData.SetScoreValue(newScore, true);
        }
    }
}
