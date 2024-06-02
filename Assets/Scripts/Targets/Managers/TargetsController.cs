using System;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Signals;
using Targets.Managers.Spawn;
using UnityEngine;
using Zenject;

namespace Targets.Managers
{
    public class TargetsController : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        public event Action<Target> TargetFinished;
        
        [SerializeField] private TargetsSpawner _targetsSpawner;
        [SerializeField] private FinishZone _finishZone;
        private List<Target> _currentTargets;

        private void Start()
        {
            _signalBus.Subscribe<LevelStartedSignal>(Initialize);
            _signalBus.Subscribe<LevelResetSignal>(ResetController);
        }

        private void Initialize(LevelStartedSignal signal)
        {
            _currentTargets = new List<Target>();
            Target.TargetClicked += OnTargetClicked;
            _targetsSpawner.Initialize(signal.LevelData.TargetsSpawnData, signal.LevelData.LevelTargetsStats);
            
            _targetsSpawner.TargetSpawned += OnTargetSpawned;
            _finishZone.TargetTriggeredFinish += OnTargetTriggeredFinish;
        }
        
        private void OnTargetSpawned(Target target)
        {
            _currentTargets.Add(target);
        }
        
        private void OnTargetTriggeredFinish(Collider2D targetCol)
        {
            var target = _currentTargets.FirstOrDefault(x => x.Collider == targetCol);
            if (target!=null)
            {
                _currentTargets.Remove(target);
                LeanPool.Despawn(target);
                TargetFinished?.Invoke(target);
            }
        }

        private void OnTargetClicked(Target target)
        {
            if (_currentTargets.Contains(target))
            {
                _currentTargets.Remove(target);
                LeanPool.Despawn(target);
            }
        }

        private void ResetController()
        {
            _targetsSpawner.ResetSpawner();
            for (int i = 0, count = _currentTargets.Count; i < count; i++)
            {
                LeanPool.Despawn(_currentTargets[i]);
            }
            
            _currentTargets = null;
            Target.TargetClicked -= OnTargetClicked;
            _targetsSpawner.TargetSpawned -= OnTargetSpawned;
            _finishZone.TargetTriggeredFinish -= OnTargetTriggeredFinish;
        }
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            _finishZone ??= FindObjectOfType<FinishZone>();
        }

#endif
    }
}
