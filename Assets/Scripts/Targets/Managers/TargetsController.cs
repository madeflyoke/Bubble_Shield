using System;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Targets.Managers.Spawn;
using Targets.Utility;
using UnityEngine;

namespace Targets.Managers
{
    public class TargetsController : MonoBehaviour
    {
        public event Action<Target> TargetFinished;
        
        [SerializeField] private TargetsSpawner _targetsSpawner;
        [SerializeField] private FinishZone _finishZone;
        private List<Target> _currentTargets;
        
        public void Initialize(TargetsSpawnData spawnData, LevelTargetStats levelTargetStats)
        {
            _currentTargets = new List<Target>();
            Target.TargetClicked += OnTargetClicked;
            _targetsSpawner.Initialize(spawnData, levelTargetStats);
        }

        private void OnEnable()
        {
            _targetsSpawner.TargetSpawned += OnTargetSpawned;
            _finishZone.TargetTriggeredFinish += OnTargetTriggeredFinish;
        }
        
        private void OnDisable()
        {
            _targetsSpawner.TargetSpawned -= OnTargetSpawned;
            _finishZone.TargetTriggeredFinish -= OnTargetTriggeredFinish;
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
        
        #if UNITY_EDITOR

        private void OnValidate()
        {
            _finishZone ??= FindObjectOfType<FinishZone>();
        }

#endif
    }
}
