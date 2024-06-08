using System;
using System.Collections.Generic;
using System.Linq;
using Finish;
using Lean.Pool;
using Signals;
using Slicing;
using Targets.Managers.Spawn;
using UnityEngine;
using Zenject;

namespace Targets.Managers
{
    public class TargetsController : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        public event Action<Target> TargetFinished;
        public event Action<Target> TargetKilled;

        [SerializeField] private TargetsSpawner _targetsSpawner;
        [SerializeField] private FinishZone _finishZone;
        private Dictionary<GameObject, Target> _currentTargetsMap;

        private void Awake()
        {
            _signalBus.Subscribe<MatchStartedSignal>(Initialize);
            _signalBus.Subscribe<ResetMatchSignal>(ResetController);
        }

        private void Initialize(MatchStartedSignal signal)
        {
            _currentTargetsMap = new Dictionary<GameObject, Target>();

            _targetsSpawner.Initialize(signal.MatchData.DifficultiesData, signal.MatchData.SpawnPointsCount);
            
            _targetsSpawner.TargetSpawned += OnTargetSpawned;
            
            TargetsSlicer.TargetSliced += OnTargetSliced;
            _finishZone.TargetTriggeredFinish += OnTargetTriggeredFinish;
        }
        
        private void OnTargetSpawned(Target target)
        {
            AddTarget(target);
        }
        
        private void OnTargetTriggeredFinish(GameObject targetGo)
        {
            if (_currentTargetsMap.ContainsKey(targetGo))
            {
                var target = _currentTargetsMap[targetGo];
                RemoveTarget(target);
                target.OnFinishHide(() =>
                {
                    TargetFinished?.Invoke(target);
                });
            }
        }

        private void OnTargetSliced(GameObject targetGo)
        {
            if (_currentTargetsMap.ContainsKey(targetGo))
            {
                var target = _currentTargetsMap[targetGo];
                RemoveTarget(target);
                
                target.OnTouchedHide(() =>
                {
                    TargetKilled?.Invoke(target);
                });
            }
        }

        private void AddTarget(Target target)
        {
            _currentTargetsMap.Add(target.gameObject, target);
        }

        private void RemoveTarget(Target target)
        {
            _currentTargetsMap.Remove(target.gameObject);
        }
        
        private void ResetController()
        {
            _targetsSpawner.ResetSpawner();
            var targetsList = _currentTargetsMap.Keys.ToList();
            for (int i = 0, count = targetsList.Count; i < count; i++)
            {
                var target = targetsList[i];
                LeanPool.Despawn(target);
            }
            
            _currentTargetsMap = null;
            _targetsSpawner.TargetSpawned -= OnTargetSpawned;
            TargetsSlicer.TargetSliced -= OnTargetSliced;
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
