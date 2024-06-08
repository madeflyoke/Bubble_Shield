using System;
using System.Collections.Generic;
using System.Linq;
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

        private void Awake()
        {
            _screens = new List<IScreen>
            {
                _mainMenuScreen,
                _gameplayScreen
            };
            _signalBus.Subscribe<MatchStartedSignal>(OnMatchStarted);
        }

        private void Start()
        {
            ShowSingleScreen<MainMenuScreen>();
        }

        public T GetScreen<T>() where T : IScreen
        {
            return (T)_screens.FirstOrDefault(x => x.GetType() == typeof(T));
        }
        
        private void OnMatchStarted(MatchStartedSignal _)
        {
            ShowSingleScreen<GameplayScreen>();
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
