using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Levels.Configs
{
    [CreateAssetMenu(menuName = "Levels/LevelConfig", fileName = "LevelConfig")]
    public class LevelsConfig : ScriptableObject, ISerializationCallbackReceiver
    {
        public int LevelsCount => _levels.Count;
        
        [SerializeField] private List<LevelData> _levels;

        public LevelData GetLevelData(int levelId)
        {
            return _levels.FirstOrDefault(x => x.Id == levelId);
        }
        
        public void OnBeforeSerialize()
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                var levelData = _levels[i];
                levelData.Id = i;
                
                if (levelData.TargetsSpawnData.CollumnsCount < levelData.TargetsSpawnData.TargetsPerSpawn)
                {
                    levelData.TargetsSpawnData.CollumnsCount = levelData.TargetsSpawnData.TargetsPerSpawn;
                }
                
                _levels[i] = levelData;
            }
        }

        public void OnAfterDeserialize()
        {
           
        }
    }
}
