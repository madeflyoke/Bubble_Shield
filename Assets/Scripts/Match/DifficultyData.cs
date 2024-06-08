using System;
using Targets.Managers.Spawn;
using Targets.Utility;

namespace Match
{
    [Serializable]
    public class DifficultyData
    {
        public int Index;
        public int ScoreToNextDifficulty;
        public TargetsSpawnData TargetsSpawnData;
        public TargetStats TargetsStats;
    }
}
