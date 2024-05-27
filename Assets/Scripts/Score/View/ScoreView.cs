using TMPro;
using UnityEngine;

namespace Score.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _targetScoreText;

        public void SetCurrentScore(int value)
        {
            _scoreText.text = value.ToString();
        }

        public void SetTargetScore(int value)
        {
            _targetScoreText.text = value.ToString();
        }
    }
}
