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
        
        [field: SerializeField] public MainMenuScreen MainMenuScreen { get; private set; }
        [field: SerializeField] public GameplayScreen GameplayScreen { get; private set; }
        private List<IScreen> _screens;

        private void Awake()
        {
            _screens = new List<IScreen>
            {
                MainMenuScreen,
                GameplayScreen
            };
            _signalBus.Subscribe<MatchStartedSignal>(OnMatchStarted);
        }

        private void Start()
        {
            ShowSingleScreen<MainMenuScreen>();
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
