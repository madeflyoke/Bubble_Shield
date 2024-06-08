using Signals;
using UI.Screens.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Screens
{
    public class MainMenuScreen : MonoBehaviour, IScreen
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private Button _startButton;

        public void Start()
        {
            _startButton.gameObject.SetActive(true);
            
            _startButton.onClick.AddListener(()=> //start button showed up only once at start
            {
                _startButton.enabled = false;
                _startButton.onClick.RemoveAllListeners();
                _signalBus.Fire<StartMatchCallSignal>();
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
