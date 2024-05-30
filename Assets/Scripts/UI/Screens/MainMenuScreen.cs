using System;
using UI.LevelSelection;
using UI.Screens.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class MainMenuScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private LevelSelector _levelSelector;

        public void Start()
        {
            _startButton.gameObject.SetActive(true);
            _levelSelector.SetActive(false);
            _levelSelector.Initialize();
            
            _startButton.onClick.AddListener(()=> //start button showed up only once at start
            {
                _levelSelector.SetActive(true);
                _startButton.enabled = false;
                _startButton.onClick.RemoveAllListeners();
            });
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
