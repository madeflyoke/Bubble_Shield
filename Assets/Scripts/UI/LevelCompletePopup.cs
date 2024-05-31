using System;
using System.Collections.Generic;
using Managers;
using Services;
using Signals;
using UI.Screens.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LevelCompletePopup : MonoBehaviour, IScreen
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private ServicesHolder _servicesHolder;

        [SerializeField] private List<FinishStar> _finishStars;
        [SerializeField] private Button _backToSelectorButton;
        
        public void Show()
        {
            _backToSelectorButton.onClick.AddListener(BackToSelector);
            _servicesHolder.GetService<PauseService>().SetPause(true);
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
            _servicesHolder.GetService<PauseService>().SetPause(false);
            _backToSelectorButton.onClick.RemoveListener(BackToSelector);
        }
        
        private void BackToSelector()
        {
            _signalBus.Fire<LevelSelectorCallSignal>();
            Hide();
        }

        [Serializable]
        public class FinishStar
        {
            public Image OnSprite;
            public Image OffSprite;

            public void SetActive(bool isActive)
            {
                OnSprite.gameObject.SetActive(isActive);
                OffSprite.gameObject.SetActive(!isActive);
            }
        }
    }
}
