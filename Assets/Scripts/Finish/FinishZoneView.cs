using System;
using DG.Tweening;
using EasyButtons;
using TMPro;
using UniRx;
using UnityEngine;

namespace Finish
{
    public class FinishZoneView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private Color _increaseColor;
        [SerializeField] private Color _decreaseColor;
        private Color _defaultColor;
        private Tween _tween;

        private void Awake()
        {
            _defaultColor = _healthText.color;
        }

        public void LinkReactProperty(IntReactiveProperty health)
        {
            health.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(SetHealthText).AddTo(this);
        }
        
        private void SetHealthText(int value)
        {
            _healthText.text = value.ToString();
        }

        public void ShowChangeEffect(bool isIncrease)
        {
            _tween?.Kill();
            _tween = _healthText.DOColor(isIncrease ? _increaseColor : _decreaseColor, 0.15f).SetEase(Ease.Linear)
                .SetLoops(2, LoopType.Yoyo).OnKill(()=>_healthText.color = _defaultColor);
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }
    }
}
