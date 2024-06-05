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
        private Dictionary<Collider2D, Target> _currentTargetsMap;

        private void Start()
        {
            _signalBus.Subscribe<LevelStartedSignal>(Initialize);
            _signalBus.Subscribe<LevelResetSignal>(ResetController);
        }

        private void Initialize(LevelStartedSignal signal)
        {
            _currentTargetsMap = new Dictionary<Collider2D, Target>();
            TargetsBlade.TargetTouched += OnTargetTouched;
            _targetsSpawner.Initialize(signal.LevelData.TargetsSpawnData, signal.LevelData.LevelTargetsStats);
            
            _targetsSpawner.TargetSpawned += OnTargetSpawned;
            _finishZone.TargetTriggeredFinish += OnTargetTriggeredFinish;
        }
        
        private void OnTargetSpawned(Target target)
        {
            _currentTargetsMap.Add(target.Collider, target);
        }
        
        private void OnTargetTriggeredFinish(Collider2D targetCol)
        {
            var target = _currentTargetsMap[targetCol];
            if (target!=null)
            {
                _currentTargetsMap.Remove(targetCol);
                target.OnFinishHide(() =>
                {
                    TargetFinished?.Invoke(target);
                });
            }
        }

        private void OnTargetTouched(Collider2D targetCol)
        {
            if (_currentTargetsMap.ContainsKey(targetCol))
            {
                var target = _currentTargetsMap[targetCol];
                _currentTargetsMap.Remove(targetCol);
                target.OnTouchedHide(() =>
                {
                    LeanPool.Despawn(targetCol.gameObject);
                });
            }
        }
        
        private void ResetController()
        {
            _targetsSpawner.ResetSpawner();
            var targetsList = _currentTargetsMap.Values.ToList();
            for (int i = 0, count = targetsList.Count; i < count; i++)
            {
                LeanPool.Despawn(targetsList[i]);
            }
            
            _currentTargetsMap = null;
            TargetsBlade.TargetTouched -= OnTargetTouched;
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
