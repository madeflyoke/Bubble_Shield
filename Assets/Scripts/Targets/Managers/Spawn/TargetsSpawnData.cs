using System;
using UnityEngine;

namespace Targets.Managers.Spawn
{
    [Serializable]
    public struct TargetsSpawnData
    {
        public float TargetSpawnDelay;
        public int TargetsPerSpawn;
        [SerializeField] public int AvarageAllyPerWaveSpawnRatio;
    }
}
