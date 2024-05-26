using System;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Targets;
using UnityEngine;

namespace Managers
{
    public class TargetsController : MonoBehaviour
    {
        public event Action<Target> TargetFinished;
        
        [SerializeField] private TargetsSpawner _targetsSpawner;
        [SerializeField] private FinishZone _finishZone;
        private List<Target> _currentTargets;
        
        private void Start()
        {
            _currentTargets = new List<Target>();
            _targetsSpawner.Spawn();
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
    }
}
