using System;
using Targets.Managers.Spawn;
using Targets.Utility;

namespace Levels
{
    [Serializable]
    public struct LevelData
    {
        public int Id;
        public int TargetScore;
        public TargetsSpawnData TargetsSpawnData;
        public LevelTargetStats LevelTargetsStats;
    }
}
