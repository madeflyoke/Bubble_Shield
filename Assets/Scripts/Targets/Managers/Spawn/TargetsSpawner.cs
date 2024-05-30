using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Signals;
using Targets.Enums;
using Targets.Tools;
using Targets.Utility;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Targets.Managers.Spawn
{
    public class TargetsSpawner : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        public event Action<Target> TargetSpawned;
        
        [SerializeField] private TargetsFactory _targetsFactory;
        [SerializeField] private SpawnPointsCreator _spawnPointsCreator;
        [SerializeField] private RectTransform _targetsHolder;

        private List<Vector3> _currentSpawnPoints;
        private RectTransform _parent;

        private CancellationTokenSource _cts;

        private void Start()
        {
            _signalBus.Subscribe<LevelSelectorCallSignal>(ResetSpawner);
        }

        public void Initialize(TargetsSpawnData spawnData, LevelTargetStats levelTargetStats)
        {
            _cts = new CancellationTokenSource();
            
            var spawnPoints = _spawnPointsCreator.CreateSpawnPoints(spawnData.CollumnsCount, _targetsHolder, out Vector3 targetCalculatedScale);
            _currentSpawnPoints = spawnPoints.ToList(); //copy
            
            _targetsFactory.SetCurrentSpecifications(levelTargetStats, targetCalculatedScale);
            StartSpawnCycle(spawnData, targetCalculatedScale.y);
        }
        
        private async void StartSpawnCycle(TargetsSpawnData spawnData, float targetScaleHeight)
        {
            int currentTargetIndex = 0; //prepare local scope variables
            int allyNextSpawnIndex = 0;
            Target currentHighestTarget =null;
            Vector3 lastSpawnPoint = _currentSpawnPoints[^1]; 

            while (true)
            {
                var isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(spawnData.TargetSpawnDelay), cancellationToken:_cts.Token).SuppressCancellationThrow();
                if (isCanceled)
                    return;
                
                _currentSpawnPoints.ShuffleWithoutLastRepeat(lastSpawnPoint); //shuffle

                Target iterationHighestElement = null;
                Vector3 spawnPoint = default;
                
                for (int i = 0; i < spawnData.TargetsPerSpawn; i++)
                {
                    spawnPoint = _currentSpawnPoints[i];
                    
                    spawnPoint.y = GetCorrectedYPos(currentHighestTarget, spawnPoint.y, targetScaleHeight); //spawn guarantee upper than highest target
                    
                    TargetVariant variant;
                    if (currentTargetIndex == allyNextSpawnIndex)
                    {
                        variant = TargetVariant.ALLY;
                        allyNextSpawnIndex += spawnData.AvarageAllySpawnRatio + Random.Range(-1, 1);
                    }
                    else
                    {
                        variant = TargetVariant.ENEMY;
                    }

                    var target = SpawnTarget(spawnPoint, variant);
                    currentTargetIndex++;
                    
                    if (i==0)
                    {
                        iterationHighestElement = target;
                    }
                    else
                    {
                        if (spawnPoint.y>iterationHighestElement.transform.position.y)
                        {
                            iterationHighestElement = target;
                        }
                    }
                }

                lastSpawnPoint = spawnPoint;
                currentHighestTarget = iterationHighestElement;
            }
        }

        private float GetCorrectedYPos(Target highestTarget, float targetYPos, float targetScaleHeight)
        {
            var newPosY = targetYPos + Random.Range(-1f, 1f);
                    
            if (highestTarget != null)
            {
                var limitedY = highestTarget.transform.position.y + targetScaleHeight;
                newPosY = Mathf.Clamp(newPosY, limitedY, newPosY);
            }

            return newPosY;
        }
        
        private Target SpawnTarget(Vector3 position, TargetVariant variant)
        {
            var target =_targetsFactory.CreateTarget(new TargetsFactory.TargetSpawnData()
            {
                Parent = _targetsHolder,
                Position = position,
                Variant = variant
            });
                    
            TargetSpawned?.Invoke(target);
            return target;
        }

        private void ResetSpawner(LevelSelectorCallSignal _)
        {
            _cts?.Cancel();
            
        }
        
        private void OnDisable()
        {
            _cts?.Cancel();
        }
    }
}
