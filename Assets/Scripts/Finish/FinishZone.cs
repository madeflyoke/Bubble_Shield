using System;
using Signals;
using Targets;
using Targets.Enums;
using Targets.Managers;
using UniRx;
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

        private IntReactiveProperty _currentHealth;
        private int _maxHealth;

        private void Awake()
        {
            _currentHealth = new IntReactiveProperty();
            _finishZoneView.LinkReactProperty(_currentHealth);

            _signalBus.Subscribe<MatchStartedSignal>(Initialize);
            _signalBus.Subscribe<ResetMatchSignal>(ResetZone);
        }

        private void Initialize(MatchStartedSignal signal)
        {
            _maxHealth = signal.MatchData.MaxHealth;
            _currentHealth.Value = _maxHealth;
            _targetsController.TargetFinished += OnTargetFinished;
        }
        
        private void OnTargetFinished(Target target)
        {
            switch (target.Variant)
            {
                case TargetVariant.ENEMY:
                    _currentHealth.Value--;
                    break;
                case TargetVariant.ALLY:
                    _currentHealth.Value++;
                    break;
            }

            _currentHealth.Value = Mathf.Clamp(_currentHealth.Value, 0, _maxHealth);
            
            if (_currentHealth.Value==0)
            {
                _signalBus.Fire(new MatchCompletedSignal());
                _targetsController.TargetFinished -= OnTargetFinished;
            }
        }

        private void ResetZone()
        {
            _targetsController.TargetFinished -= OnTargetFinished;
            _currentHealth.Value = 0;
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            TargetTriggeredFinish?.Invoke(col.gameObject);
        }
    }
}
