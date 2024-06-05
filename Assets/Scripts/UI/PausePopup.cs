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
        [SerializeField] private BackToLevelSelectorButton _backToLevelSelectorButton;
        [SerializeField] private RestartLevelButton _restartLevelButton;
        [SerializeField] private PopupAnimator _animator;
        
        public void Show()
        {
            IsShowed = true;

            _closeButton.onClick.AddListener(Hide);
            _servicesHolder.GetService<PauseService>().SetPause(true);
            
            _backToLevelSelectorButton.Enable(Hide);
            _restartLevelButton.Enable(Hide);

            _animator.PlayShowAnimation();
        }
        
        public void Hide()
        {
            _backToLevelSelectorButton.Disable();
            _restartLevelButton.Disable();
            
            _closeButton.onClick.RemoveListener(Hide);
            
            _animator.PlayHideAnimation(() =>
            {
                _servicesHolder.GetService<PauseService>().SetPause(false);
            });
            IsShowed = false;
        }
    }
}
