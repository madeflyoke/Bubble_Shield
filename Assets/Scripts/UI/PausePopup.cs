using System;
using Managers;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PausePopup : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private PauseManager _pauseManager;
        
        [SerializeField] private Button _backToSelectorButton;
        [SerializeField] private Button _closeButton;

        private void Start()
        {
            Hide();
            _closeButton.onClick.AddListener(Hide);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _backToSelectorButton.onClick.AddListener(BackToSelector);
            _pauseManager.SetPause(true);
        }
        
        public void Hide()
        {
            _pauseManager.SetPause(false);
            gameObject.SetActive(false);
            _backToSelectorButton.onClick.RemoveListener(BackToSelector);
        }
        
        private void BackToSelector()
        {
            _signalBus.Fire<LevelSelectorCallSignal>();
            Hide();
        }
    }
}
