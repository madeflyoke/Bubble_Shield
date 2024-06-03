using System;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class RestartLevelButton : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private Button _button;
        
        public void Enable(Action onClickCallback)
        {
            _button.enabled = true;
            _button.onClick.AddListener(()=>
            {
                _button.enabled = false;
                CallOnLevelRestart();
                onClickCallback?.Invoke();
            });
        }

        public void Disable()
        {
            _button.onClick.RemoveAllListeners();
        }
        
        private void CallOnLevelRestart()
        {
            _signalBus.Fire<LevelResetSignal>();
            _signalBus.Fire<RestartLevelCallSignal>();
        }
    }
}
