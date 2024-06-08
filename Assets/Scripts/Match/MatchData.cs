using System;
using System.Collections.Generic;
using System.Linq;

namespace Match
{
    [Serializable]
    public struct MatchData
    {
        public int MaxHealth;
        public int SpawnPointsCount;
        public List<DifficultyData> DifficultiesData;
    }
}
