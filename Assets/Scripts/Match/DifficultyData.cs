using System;
using Targets.Managers.Spawn;
using Targets.Utility;

namespace Match
{
    [Serializable]
    public class DifficultyData
    {
        public int Index;
        public int TargetsKillToNextDifficulty;
        public TargetsSpawnData TargetsSpawnData;
        public TargetStats TargetsStats;
    }
}
