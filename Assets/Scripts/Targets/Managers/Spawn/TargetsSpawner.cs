using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Match;
using Score.Controller;
using Targets.Enums;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Targets.Managers.Spawn
{
    public class TargetsSpawner : MonoBehaviour
    {
        public event Action<Target> TargetSpawned;
        
        [SerializeField] private float _spawnHeight =6f;
        [SerializeField] private float _sidesSpawnPadding=0.75f;
        
        [SerializeField] private TargetsFactory _targetsFactory;
        [SerializeField] private RectTransform _targetsHolder;
        [SerializeField] private ScoreController _scoreController;

        private List<DifficultyData> _difficultyDatas;
        private DifficultyData _currentDifficultyData;

        private List<Vector3> _currentSpawnPoints;

        private bool _nextDifficultyCalled;
        
        private CancellationTokenSource _cts;
        private UniTask _spawnTask;
        
        private float _targetCalculatedHeight;

        public void Initialize(List<DifficultyData> difficultiesData, int spawnPointsCount)
        {
            _cts = new CancellationTokenSource();
            _scoreController.CurrentScoreChanged += OnCurrentScoreChanged;
            
            var spawnPoints = new SpawnPointsCreator(_spawnHeight,  _sidesSpawnPadding, spawnPointsCount, _targetsHolder)
                .CreateSpawnPoints(out Vector3 targetCalculatedScale);
            _currentSpawnPoints = spawnPoints.ToList(); //copy
            
            _targetCalculatedHeight = targetCalculatedScale.y;
            _targetsFactory.SetCommonSpecifications(targetCalculatedScale);

            _difficultyDatas = difficultiesData; //TODO HERE
            SetCurrentDifficultyData(0);
        }

        private void SetCurrentDifficultyData(int index)
        {
            _currentDifficultyData = _difficultyDatas[index];
            
            _targetsFactory.SetCurrentSpecifications(_currentDifficultyData.TargetsStats);
            _spawnTask = StartSpawnCycle();
        }
        
        private async void OnCurrentScoreChanged(int score)
        {
            if (_currentDifficultyData.ScoreToNextDifficulty <= score)
            {
                _scoreController.CurrentScoreChanged -= OnCurrentScoreChanged;
                _nextDifficultyCalled = true;
                var nextIndex = Mathf.Clamp(_difficultyDatas.IndexOf(_currentDifficultyData)+1, 0, _difficultyDatas.Count-1);

                await _spawnTask;
                
                _cts = new CancellationTokenSource();
                _scoreController.CurrentScoreChanged += OnCurrentScoreChanged;
                _nextDifficultyCalled = false;
                SetCurrentDifficultyData(nextIndex);
            }
        }

        private async UniTask StartSpawnCycle()
        {
            Debug.LogWarning("Start new cycle");
            var spawnData = _currentDifficultyData.TargetsSpawnData;
            
            int currentWaveIndex = 0; //prepare local scope variables
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
                    spawnPoint.y = GetCorrectedYPos(currentHighestTarget, spawnPoint.y); //spawn guarantee upper than highest target
                    
                    TargetVariant variant;
                    if (currentWaveIndex == allyNextSpawnIndex)
                    {
                        variant = TargetVariant.ALLY;
                        allyNextSpawnIndex += spawnData.AvarageAllyPerWaveSpawnRatio + Random.Range(-1, 1);
                    }
                    else
                    {
                        variant = TargetVariant.ENEMY;
                    }

                    var target = SpawnTarget(spawnPoint, variant);
                    
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
                
                currentWaveIndex++;
                lastSpawnPoint = spawnPoint;
                currentHighestTarget = iterationHighestElement;

                if (_nextDifficultyCalled)
                {
                    return;
                }
            }
        }

        private float GetCorrectedYPos(Target highestTarget, float targetYPos)
        {
            var newPosY = targetYPos + Random.Range(-1f, 1f);
                    
            if (highestTarget != null)
            {
                var limitedY = highestTarget.transform.position.y + _targetCalculatedHeight;
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

        public void ResetSpawner()
        {
            _cts?.Cancel();
            _scoreController.CurrentScoreChanged -= OnCurrentScoreChanged;
        }
        
        private void OnDisable()
        {
            _cts?.Cancel();
        }
    }
}
