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
        private List<RectTransform> _currentSpawnPoints;

        private CancellationTokenSource _cts;

        private void Start()
        {
            _signalBus.Subscribe<LevelSelectorCallSignal>(ResetSpawner);
        }

        public void Initialize(TargetsSpawnData spawnData, LevelTargetStats levelTargetStats)
        {
            _cts = new CancellationTokenSource();
            
            var spawnPoints = _spawnPointsCreator.CreateSpawnPoints(spawnData.CollumnsCount, out Vector3 targetCalculatedScale);
            _currentSpawnPoints = spawnPoints.ToList(); //copy
            
            _targetsFactory.SetCurrentSpecifications(levelTargetStats, targetCalculatedScale);
            StartSpawnCycle(spawnData, targetCalculatedScale.y);
        }
        
        private async void StartSpawnCycle(TargetsSpawnData spawnData, float targetScaleHeight)
        {
            int currentTargetIndex = 0; //prepare local scope variables
            int allyNextSpawnIndex = 0;
            Target currentHighestTarget =null;
            RectTransform lastSpawnPoint = _currentSpawnPoints[^1]; 

            while (true)
            {
                var isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(spawnData.TargetSpawnDelay), cancellationToken:_cts.Token).SuppressCancellationThrow();
                if (isCanceled)
                    return;
                
                _currentSpawnPoints.ShuffleWithoutLastRepeat(lastSpawnPoint); //shuffle

                Target iterationHighestElement = null;
                RectTransform spawnPoint = null;
                
                for (int i = 0; i < spawnData.TargetsPerSpawn; i++)
                {
                    spawnPoint = _currentSpawnPoints[i];
                    var spawnPosition = spawnPoint.position;
                    
                    spawnPosition.y = GetCorrectedYPos(currentHighestTarget, spawnPosition.y, targetScaleHeight); //spawn guarantee upper than highest target
                    
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

                    var target = SpawnTarget(spawnPoint, spawnPosition, variant);
                    currentTargetIndex++;
                    
                    if (i==0)
                    {
                        iterationHighestElement = target;
                    }
                    else
                    {
                        if (spawnPosition.y>iterationHighestElement.transform.position.y)
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
        
        private Target SpawnTarget(Transform parent, Vector3 position, TargetVariant variant)
        {
            var target =_targetsFactory.CreateTarget(new TargetsFactory.TargetSpawnData()
            {
                Parent = parent,
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
