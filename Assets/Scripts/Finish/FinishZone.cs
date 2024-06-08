using System;
using Signals;
using Targets;
using Targets.Enums;
using Targets.Managers;
using UnityEngine;
using Zenject;

namespace Finish
{
    public class FinishZone : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private TargetsController _targetsController;

        public event Action<GameObject> TargetTriggeredFinish;

        [SerializeField] private FinishZoneView _finishZoneView;
        private int _currentHealth;
        private int _maxHealth;

        private void Awake()
        {
            _signalBus.Subscribe<MatchStartedSignal>(Initialize);
            _signalBus.Subscribe<ResetMatchSignal>(ResetZone);
        }

        private void Initialize(MatchStartedSignal signal)
        {
            _maxHealth = signal.MatchData.MaxHealth;
            _currentHealth = _maxHealth;
            _finishZoneView.SetHealthText(_currentHealth);
            
            _targetsController.TargetFinished += OnTargetFinished;
        }
        
        private void OnTargetFinished(Target target)
        {
            switch (target.Variant)
            {
                case TargetVariant.ENEMY:
                    _currentHealth--;
                    break;
                case TargetVariant.ALLY:
                    _currentHealth++;
                    break;
            }

            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            _finishZoneView.SetHealthText(_currentHealth);
            
            if (_currentHealth==0)
            {
                _signalBus.Fire(new MatchCompletedSignal());
                _targetsController.TargetFinished -= OnTargetFinished;
            }
        }

        private void ResetZone()
        {
            _targetsController.TargetFinished -= OnTargetFinished;
            _currentHealth = 0;
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            TargetTriggeredFinish?.Invoke(col.gameObject);
        }
    }
}
