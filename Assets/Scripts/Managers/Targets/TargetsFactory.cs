using Lean.Pool;
using Targets;
using Targets.Configs;
using Targets.Enums;
using Targets.Utility;
using UnityEngine;

namespace Managers.Targets
{
    public class TargetsFactory : MonoBehaviour
    {
        [SerializeField] private Target targetPrefab;
        [SerializeField] private TargetsConfig _targetsConfig;
        
        public Target CreateTarget(TargetSpawnData spawnData)
        {
            var target = LeanPool.Spawn(targetPrefab, spawnData.Position, Quaternion.identity, spawnData.Parent);
            target.transform.localScale = spawnData.Scale;
            target.Initialize(new TargetData()
            {
                Variant = spawnData.Variant,
                Sprite = _targetsConfig.GetRandomSprite(spawnData.Variant),
                Speed = _targetsConfig.DefaultSpeed
            });
            return target;
        }

        public struct TargetSpawnData
        {
            public Transform Parent;
            public Vector3 Position;
            public Vector3 Scale;
            public TargetVariant Variant;
        }
    }
}
