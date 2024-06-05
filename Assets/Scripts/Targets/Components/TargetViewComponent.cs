using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Targets.Components
{
    public class TargetViewComponent : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private Vector3 _defaultImageScale;

        private void Start()
        {
            _defaultImageScale = _image.transform.localScale;
        }

        public void Initialize(Sprite sprite)
        {
            SetSprite(sprite);
        }

        public void SetHideAnimation(Action onComplete)
        {
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
            _image.transform.localScale = _defaultImageScale;
        }
    }
}
