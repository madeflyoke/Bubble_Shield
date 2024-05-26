using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using EasyButtons;
using Targets;
using Targets.Enums;
using Targets.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.Targets
{
    public class TargetsSpawner : MonoBehaviour
    {
        public event Action<Target> TargetSpawned;
        
        [SerializeField] private TargetsFactory _targetsFactory;
        [SerializeField] private List<RectTransform> _spawnPositions;
        [SerializeField] private int _targetsCount;
        [SerializeField] private float _targetSpawnDelay;
        [SerializeField] private int _targetsPerSpawn;
        [SerializeField, Range(0,1)] private float _allySpawnChance = 0.1f;
        [SerializeField] private Vector3 _targetCalculatedScale;
        private List<RectTransform> _spawnPositionsShuffled;
        private CancellationTokenSource _cts;
        private Target _currentHighestElement;

        public void Awake()
        {
            _cts = new CancellationTokenSource();
            _spawnPositionsShuffled = _spawnPositions.ToList();
            _spawnPositionsShuffled.Shuffle();
        }

        public async void Spawn()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_targetSpawnDelay), cancellationToken:_cts.Token);
                
                _spawnPositionsShuffled.Shuffle();

                Target queueHighestElement = null;
                
                for (int i = 0; i < _targetsPerSpawn; i++)
                {
                    var spawnPoint = _spawnPositionsShuffled[i];
                    var spawnPosition = spawnPoint.position;
                    var newPosY = spawnPosition.y + Random.Range(-2f, 2f);
                    
                    if (_currentHighestElement != null) //guarantee to element will be higher than previous
                    {
                        var limitedY = _currentHighestElement.transform.position.y + _targetCalculatedScale.y;
                        newPosY = Mathf.Clamp(newPosY, limitedY, newPosY);
                    }
                    
                    spawnPosition.y = newPosY;
                  
                    var target =_targetsFactory.CreateTarget(new TargetsFactory.TargetSpawnData()
                    {
                        Parent = spawnPoint,
                        Position = spawnPosition,
                        Scale = _targetCalculatedScale,
                        Variant = Random.Range(0f,1f)<_allySpawnChance? TargetVariant.ALLY: TargetVariant.ENEMY
                    });
                    
                    TargetSpawned?.Invoke(target);
                    
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

                _currentHighestElement = queueHighestElement;
            }
        }
        
        private void OnDisable()
        {
            _cts?.Cancel();
        }

        #if UNITY_EDITOR
        
        [Header("EDITOR")]
        [SerializeField] private float EDITOR_spawnHeight =6f;
        [Header("EDITOR")]
        [SerializeField] private float EDITOR_spawnPadding=0.75f;
        [Header("EDITOR")]
        [SerializeField] private RectTransform EDITOR_spawnPointsHolder;

        [Button]
        private void SetSpawnPoints()
        {
            for (int i = EDITOR_spawnPointsHolder.childCount-1; i >= 0; i--)
            {
                DestroyImmediate(EDITOR_spawnPointsHolder.GetChild(i).gameObject);
            }
            _spawnPositions = new List<RectTransform>();
            
            var minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0f));
            var maxX =Camera.main.ViewportToWorldPoint(new Vector3(1, 0f));
            
            var leftPos = new Vector3(minX.x, EDITOR_spawnHeight, 0f);
            var rightPos = new Vector3(maxX.x, EDITOR_spawnHeight, 0f);
            var step = (Mathf.Abs(rightPos.x - leftPos.x)-EDITOR_spawnPadding*2f) / (_targetsCount-1);
            var minSize = Mathf.Min(EDITOR_spawnPadding * 2f, step);
            _targetCalculatedScale = Vector3.one*minSize * 0.9f;
            
            for (int i = 0; i < _targetsCount; i++)
            {
                var pos = leftPos+ Vector3.right*step * i+Vector3.right*EDITOR_spawnPadding;
                pos.z = EDITOR_spawnPointsHolder.transform.position.z;
                var go = new GameObject();
                go.transform.position = pos;
                go.name = "SpawnPoint " + i;
                var rt = go.AddComponent<RectTransform>();
                go.transform.SetParent(EDITOR_spawnPointsHolder);
                _spawnPositions.Add(rt);
            }
        }
#endif
    }
}
