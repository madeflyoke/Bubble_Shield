using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Targets.Components
{
    public class TargetViewComponent : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        private Vector3 _defaultImageScale;

        private void Start()
        {
            _defaultImageScale = _image.transform.localScale;
        }

        public void Initialize(Sprite sprite, Action onButtonClicked)
        {
            SetSprite(sprite);
            _button.onClick.AddListener(()=>
            {
                onButtonClicked();
                _button.enabled = false;
                _button.onClick.RemoveAllListeners();
            });

            _button.enabled = true;
        }

        public void SetHideAnimation(Action onComplete)
        {
            _button.enabled = false;
            _image.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
        
        private void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
            _image.transform.localScale = _defaultImageScale;
        }
    }
}
