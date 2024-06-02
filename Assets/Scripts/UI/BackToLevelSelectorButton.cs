using System;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class BackToLevelSelectorButton : MonoBehaviour
    {
        public event Action ButtonPressed;
        
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private Button _button;
        
        public void Enable(Action callback)
        {
            _button.enabled = true;
            _button.onClick.AddListener(()=>
            {
                _button.enabled = false;
                BackToSelector();
                callback?.Invoke();
            });
        }

        public void Disable()
        {
            _button.onClick.RemoveAllListeners();
        }
        
        private void BackToSelector()
        {
            _signalBus.Fire<LevelSelectorCallSignal>();
            _signalBus.Fire<LevelSelectorCallSignal>();
            ButtonPressed?.Invoke();
        }
    }
}
