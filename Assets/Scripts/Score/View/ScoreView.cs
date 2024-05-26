using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        public void SetScoreText(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}
