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
        
        public bool IsFocused => !_pausePopup.IsShowed && !matchCompletePopup.IsShowed && gameObject.activeSelf;
        
        [SerializeField] private Button _pauseButton;
        [SerializeField] private PausePopup _pausePopup;
        [SerializeField] private MatchCompletePopup matchCompletePopup;

        private void Awake()
        {
            _signalBus.Subscribe<MatchCompletedSignal>(OnLevelCompleted);
        }

        private void Start()
        {
            _pausePopup.Hide();
            matchCompletePopup.Hide();
        }

        private void OnLevelCompleted(MatchCompletedSignal signal)
        {
            ShowLevelCompletePopup(signal);
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

        private void ShowLevelCompletePopup(MatchCompletedSignal signal)
        {
            matchCompletePopup.Show(signal);
        }
    }
}
