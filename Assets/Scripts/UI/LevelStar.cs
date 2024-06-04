using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelStar : MonoBehaviour
    {
        [SerializeField] private Image _onImage;
        private Tween _tween;
        private Vector3 _defaultOnImageScale;

        private void Awake()
        {
            _defaultOnImageScale = _onImage.transform.localScale;
        }

        public void Show(float delay)
        {
            _tween?.Kill();
            _tween = _onImage.transform.DOPunchScale(Vector3.one * 0.4f, 0.4f, 1).SetEase(Ease.InExpo).SetUpdate(true).SetDelay(delay).OnStart(
                () =>
                {
                    _onImage.gameObject.SetActive(true);
                });
        }

        public void Hide()
        {
            _onImage.gameObject.SetActive(false);
            _onImage.transform.localScale = _defaultOnImageScale;
        }
        
        private void OnDisable()
        {
            _tween?.Kill();
        }
    }
}