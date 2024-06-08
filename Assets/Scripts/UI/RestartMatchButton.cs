using System;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class RestartMatchButton : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private Button _button;
        
        public void Enable(Action onClickCallback)
        {
            _button.enabled = true;
            _button.onClick.AddListener(()=>
            {
                _button.enabled = false;
                CallOnMatchRestart();
                onClickCallback?.Invoke();
            });
        }

        public void Disable()
        {
            _button.onClick.RemoveAllListeners();
        }
        
        private void CallOnMatchRestart()
        {
            _signalBus.Fire<ResetMatchSignal>();
            _signalBus.Fire<StartMatchCallSignal>();
        }
    }
}
