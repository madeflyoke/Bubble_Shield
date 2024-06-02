using Managers;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PausePopup : MonoBehaviour
    {
        [Inject] private ServicesHolder _servicesHolder;
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private BackToLevelSelectorButton _button;
        
        public void Show()
        {
            _closeButton.onClick.AddListener(Hide);
            _servicesHolder.GetService<PauseService>().SetPause(true);
            _button.Enable(Hide);
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
            _servicesHolder.GetService<PauseService>().SetPause(false);
            _button.Disable();
            _closeButton.onClick.RemoveListener(Hide);
        }
    }
}
