using System;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Score
{
    public class ScoreCombo : MonoBehaviour
    {
        private const string POSTFIX = "</size>";
        private const string COMBO_PREFIX = "X <size=135%>";
        
        public int CurrentCombo { get; private set; }

        [SerializeField] private int _maxCombo = 50;
        [SerializeField] private TMP_Text _comboValueText;
        private StringBuilder _stringBuilder;
        private Tween _tween;
        private Vector3 _comboTextDefaultScale;

        public void Awake()
        {
            _comboTextDefaultScale = _comboValueText.transform.localScale;
            _stringBuilder = new StringBuilder();
            _comboValueText.gameObject.SetActive(false);
        }
        
        public void TrySetCombo(bool isScoreIncreased)
        {
            if (isScoreIncreased)
            {
                if (CurrentCombo==_maxCombo)
                {
                    return;
                }
                var newScore = Mathf.Clamp(CurrentCombo+1, 1, _maxCombo);
                if (newScore==_maxCombo)
                {
                    //do effect
                }
                
                CurrentCombo = newScore;
                _comboValueText.gameObject.SetActive(true);
                
                _stringBuilder.Clear();
                _stringBuilder.Append(COMBO_PREFIX);
                _stringBuilder.Append(CurrentCombo.ToString());
                _stringBuilder.Append(POSTFIX);

                _comboValueText.text = _stringBuilder.ToString();
                _tween?.Kill();
                _tween = _comboValueText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f).SetEase(Ease.Linear).OnKill(
                    () =>
                    {
                        _comboValueText.transform.localScale = _comboTextDefaultScale;
                    });
                return;
            }

            CurrentCombo = 0;
            _comboValueText.gameObject.SetActive(false);
        }

        public void ResetCombo()
        {
            CurrentCombo = 0;
            _comboValueText.text = String.Empty;
        }
    }
}
