using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PausePopup : MonoBehaviour
    {
        [Inject] private ServicesHolder _servicesHolder;
        
        public bool IsShowed { get; private set; }
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private RestartMatchButton _restartMatchButton;
        [SerializeField] private PopupAnimator _animator;
        
        public void Show()
        {
            IsShowed = true;

            _closeButton.onClick.AddListener(Hide);
            _servicesHolder.GetService<PauseService>().SetPause(true);
            
            _restartMatchButton.Enable(Hide);

            _animator.PlayShowAnimation();
        }
        
        public void Hide()
        {
            _restartMatchButton.Disable();
            
            _closeButton.onClick.RemoveListener(Hide);
            
            _animator.PlayHideAnimation(() =>
            {
                _servicesHolder.GetService<PauseService>().SetPause(false);
            });
            IsShowed = false;
        }
    }
}
