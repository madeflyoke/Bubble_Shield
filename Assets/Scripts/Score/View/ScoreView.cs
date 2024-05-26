using TMPro;
using UnityEngine;

namespace Score.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _scoreRecordText;

        public void SetScoreText(int value)
        {
            _scoreText.text = value.ToString();
        }

        public void SetRecordScoreText(int value)
        {
            _scoreRecordText.text = value.ToString();
        }
    }
}
