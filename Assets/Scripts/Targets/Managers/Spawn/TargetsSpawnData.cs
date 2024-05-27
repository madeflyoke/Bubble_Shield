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
        [SerializeField, Range(0,1)] public float AllySpawnChance;
    }
}
