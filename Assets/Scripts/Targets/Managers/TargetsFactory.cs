using Lean.Pool;
using Targets.Configs;
using Targets.Enums;
using Targets.Utility;
using UnityEngine;

namespace Targets.Managers
{
    public class TargetsFactory : MonoBehaviour
    {
        [SerializeField] private Target _targetPrefab;
        [SerializeField] private TargetsConfigHolder _targetsConfigHolder;
        
        private TargetStats _currentTargetsStats;
        private Vector3 _targetScale;

        private void Awake()
        {
            _targetsConfigHolder.Initialize();
        }

        public void SetCurrentSpecifications(TargetStats targetStats)
        {
            _currentTargetsStats = targetStats;
        }
        
        public void SetCommonSpecifications(Vector3 targetScale = default)
        {
            _targetScale = targetScale;
        }
        
        public Target CreateTarget(TargetSpawnData spawnData)
        {
            var target = LeanPool.Spawn(_targetPrefab, spawnData.Position, Quaternion.identity, spawnData.Parent);
            target.transform.localScale = _targetScale;
            var sprite = _targetsConfigHolder.GetRandomSprite(spawnData.Variant);
            var relatedColor = _targetsConfigHolder.GetRelatedTargetColor(sprite);
            
            target.Initialize(new TargetData()
            {
                Variant = spawnData.Variant,
                Sprite = sprite,
                RelatedColor =  relatedColor,
                Stats = _currentTargetsStats
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
