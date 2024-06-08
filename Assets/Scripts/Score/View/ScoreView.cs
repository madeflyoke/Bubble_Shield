using TMPro;
using UnityEngine;

namespace Score.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        public void SetCurrentScore(int value)
        {
            _scoreText.text = value.ToString();
        }
    }
}
