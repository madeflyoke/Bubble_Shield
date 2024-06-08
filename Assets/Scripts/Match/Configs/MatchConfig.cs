using UnityEngine;

namespace Match.Configs
{
    [CreateAssetMenu(menuName = "Match/MatchConfig", fileName = "MatchConfig")]
    public class MatchConfig : ScriptableObject, ISerializationCallbackReceiver
    {
        [field: SerializeField] public MatchData MatchData { get; private set; }
        
        public void OnBeforeSerialize()
        {
            for (int i = 0; i < MatchData.DifficultiesData.Count; i++)
            {
                var difficultyData = MatchData.DifficultiesData[i];
                difficultyData.Index = i;
                
                if (MatchData.SpawnPointsCount < difficultyData.TargetsSpawnData.TargetsPerSpawn)
                {
                    difficultyData.TargetsSpawnData.TargetsPerSpawn = MatchData.SpawnPointsCount;
                }
                
                MatchData.DifficultiesData[i] = difficultyData;
            }

            MatchData.DifficultiesData[^1].ScoreToNextDifficulty = int.MaxValue;
        }

        public void OnAfterDeserialize()
        {
           
        }
    }
}
