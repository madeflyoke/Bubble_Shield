using UI.Screens.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class GameplayScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private PausePopup _pausePopup;
        
        public void Show()
        {
            gameObject.SetActive(true);
            _pauseButton.onClick.AddListener(ShowPausePop);
        }
        public void Hide()
        {
            _pauseButton.onClick.RemoveListener(ShowPausePop);
            gameObject.SetActive(false);
        }
        
        private void ShowPausePop()
        {
            _pausePopup.Show();
        }
    }
}
