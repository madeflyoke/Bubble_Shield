using System;
using Signals;
using UI.Screens.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Screens
{
    public class GameplayScreen : MonoBehaviour, IScreen
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private Button _pauseButton;
        [SerializeField] private PausePopup _pausePopup;
        [SerializeField] private LevelCompletePopup _levelCompletePopup;

        private void Start()
        {
            _pausePopup.Hide();
            _levelCompletePopup.Hide();
            _signalBus.Subscribe<LevelCompletedSignal>(OnLevelCompleted);
        }

        private void OnLevelCompleted(LevelCompletedSignal _)
        {
            ShowLevelCompletePopup();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _pauseButton.onClick.AddListener(ShowPausePopup);
        }
        public void Hide()
        {
            _pauseButton.onClick.RemoveListener(ShowPausePopup);
            gameObject.SetActive(false);
        }
        
        private void ShowPausePopup()
        {
            _pausePopup.Show();
        }

        private void ShowLevelCompletePopup()
        {
            _levelCompletePopup.Show();
        } 
    }
}
