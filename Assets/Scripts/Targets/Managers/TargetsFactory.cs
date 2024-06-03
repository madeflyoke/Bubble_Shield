using System;
using Lean.Pool;
using Targets.Configs;
using Targets.Enums;
using Targets.Utility;
using UnityEngine;

namespace Targets.Managers
{
    public class TargetsFactory : MonoBehaviour
    {
        public Vector3 TargetScale { get; private set; }
        
        [SerializeField] private Target _targetPrefab;
        [SerializeField] private TargetsConfig _targetsConfig;
        
        private LevelTargetStats _currentLevelTargetsStats;
        
        public void SetCurrentSpecifications(LevelTargetStats levelTargetStats, Vector3 targetScale = default)
        {
            _currentLevelTargetsStats = levelTargetStats;
            TargetScale = targetScale;
        }
        
        public Target CreateTarget(TargetSpawnData spawnData)
        {
            var target = LeanPool.Spawn(_targetPrefab, spawnData.Position, Quaternion.identity, spawnData.Parent);
            target.transform.localScale = TargetScale;
            var sprite = _targetsConfig.GetRandomSprite(spawnData.Variant);
            var relatedColor = _targetsConfig.GetRelatedTargetColor(sprite);
            
            target.Initialize(new TargetData()
            {
                Variant = spawnData.Variant,
                Sprite = sprite,
                RelatedColor =  relatedColor,
                Stats = _currentLevelTargetsStats
            });
            return target;
        }

        public struct TargetSpawnData
        {
            public Transform Parent;
            public Vector3 Position;
            public TargetVariant Variant;
        }
    }
}
