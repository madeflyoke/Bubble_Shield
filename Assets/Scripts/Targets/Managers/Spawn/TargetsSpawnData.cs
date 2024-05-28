using System;
using UnityEngine;

namespace Targets.Managers.Spawn
{
    [Serializable]
    public struct TargetsSpawnData
    {
        public int CollumnsCount;
        public float TargetSpawnDelay;
        public int TargetsPerSpawn;
        [Tooltip("Ally per THIS enemies")]
        [SerializeField] public int AvarageAllySpawnRatio;
    }
}
