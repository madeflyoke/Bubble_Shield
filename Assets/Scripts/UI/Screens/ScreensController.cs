using System;
using System.Collections.Generic;
using Levels.Managers;
using Signals;
using UI.Screens.Interfaces;
using UnityEngine;
using Zenject;

namespace UI.Screens
{
    public class ScreensController : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private MainMenuScreen _mainMenuScreen;
        [SerializeField] private GameplayScreen _gameplayScreen;
        private List<IScreen> _screens;
        
        private void Start()
        {
            _screens = new List<IScreen>
            {
                _mainMenuScreen,
                _gameplayScreen
            };

            _signalBus.Subscribe<LevelSelectedSignal>(OnLevelStarted);
            _signalBus.Subscribe<LevelSelectorCallSignal>(OnLevelSelectorCall);
            ShowSingleScreen<MainMenuScreen>();
        }
        
        private void OnLevelStarted(LevelSelectedSignal _)
        {
            ShowSingleScreen<GameplayScreen>();
        }

        private void OnLevelSelectorCall(LevelSelectorCallSignal _)
        {
            ShowSingleScreen<MainMenuScreen>();
        }

        private void ShowSingleScreen<T>() where T: IScreen
        {
            foreach (var screen in _screens)
            {
                if (screen.GetType()==typeof(T))
                {
                    screen.Show();
                }
                else
                {
                    screen.Hide();
                }
            }
        }
    }
}
