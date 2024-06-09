using TMPro;
using UniRx;
using UnityEngine;

namespace Score.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        public void LinkReactProperty(IntReactiveProperty score)
        {
            score.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(SetCurrentScore).AddTo(this);
        }
        
        public void SetCurrentScore(int value)
        {
            _scoreText.text = value.ToString();
        }
    }
}
