using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Targets.Enums;
using Targets.Tools;
using Targets.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Targets.Managers.Spawn
{
    public class TargetsSpawner : MonoBehaviour
    {
        public event Action<Target> TargetSpawned;
        
        [SerializeField] private TargetsFactory _targetsFactory;
        [SerializeField] private SpawnPointsCreator _spawnPointsCreator;
        private List<RectTransform> _spawnPoints;
        private List<RectTransform> _spawnPointsShuffled;

        private Vector3 _targetCalculatedScale;
        private Target _currentHighestElement;
        private CancellationTokenSource _cts;

        private RectTransform _lastSpawnPoint;
        
        private TargetsSpawnData _spawnData;
        private LevelTargetStats _targetStats;
        private int _currentTargetIndex;
        
        public void Initialize(TargetsSpawnData spawnData, LevelTargetStats levelTargetStats)
        {
            _cts = new CancellationTokenSource();
            
            _spawnData = spawnData;
            _targetStats = levelTargetStats;
            _spawnPoints = _spawnPointsCreator.CreateSpawnPoints(spawnData.CollumnsCount, out _targetCalculatedScale);
            _spawnPointsShuffled = _spawnPoints.ToList(); //copy
            _lastSpawnPoint = _spawnPointsShuffled[^1];
            StartSpawnCycle();
        }
        
        private async void StartSpawnCycle()
        {
            int allySpawnIndex = 0;

            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_spawnData.TargetSpawnDelay), cancellationToken:_cts.Token);
                
                _spawnPointsShuffled.ShuffleWithoutLastRepeat(_lastSpawnPoint);

                Target queueHighestElement = null;
                RectTransform spawnPoint = null;
                
                for (int i = 0; i < _spawnData.TargetsPerSpawn; i++)
                {
                    spawnPoint = _spawnPointsShuffled[i];
                    var spawnPosition = spawnPoint.position;
                    var newPosY = spawnPosition.y + Random.Range(-1f, 1f);
                    
                    if (_currentHighestElement != null) //guarantee to element will be higher than previous
                    {
                        var limitedY = _currentHighestElement.transform.position.y + _targetCalculatedScale.y;
                        newPosY = Mathf.Clamp(newPosY, limitedY, newPosY);
                    }
                    
                    spawnPosition.y = newPosY;
                    
                    TargetVariant variant;
                    
                    if ( _currentTargetIndex == allySpawnIndex)
                    {
                        variant = TargetVariant.ALLY;
                        allySpawnIndex += _spawnData.AvarageAllySpawnRatio + Random.Range(-1, 1);
                    }
                    else
                    {
                        variant = TargetVariant.ENEMY;
                    }

                    var target = SpawnTarget(spawnPoint, spawnPosition, variant);
                    _currentTargetIndex++;
                    
                    if (i==0)
                    {
                        queueHighestElement = target;
                    }
                    else
                    {
                        if (spawnPosition.y>queueHighestElement.transform.position.y)
                        {
                            queueHighestElement = target;
                        }
                    }
                }

                _lastSpawnPoint = spawnPoint;
                _currentHighestElement = queueHighestElement;
            }
        }

        private Target SpawnTarget(Transform parent, Vector3 position, TargetVariant variant)
        {
            var target =_targetsFactory.CreateTarget(new TargetsFactory.TargetSpawnData()
            {
                Parent = parent,
                Position = position,
                Scale = _targetCalculatedScale,
                Variant = variant
            }, _targetStats);
                    
            TargetSpawned?.Invoke(target);
            return target;
        }
        
        private void OnDisable()
        {
            _cts?.Cancel();
        }
    }
}
