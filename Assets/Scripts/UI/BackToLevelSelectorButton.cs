using System;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class BackToLevelSelectorButton : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private Button _button;
        
        public void Enable(Action onClickCallback)
        {
            _button.enabled = true;
            _button.onClick.AddListener(()=>
            {
                _button.enabled = false;
                CallOnBackToSelector();
                onClickCallback?.Invoke();
            });
        }

        public void Disable()
        {
            _button.onClick.RemoveAllListeners();
        }
        
        private void CallOnBackToSelector()
        {
            _signalBus.Fire<LevelResetSignal>();
            _signalBus.Fire<LevelSelectorCallSignal>();
        }
    }
}