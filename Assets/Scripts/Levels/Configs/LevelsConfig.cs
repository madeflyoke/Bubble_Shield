using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Levels.Configs
{
    [CreateAssetMenu(menuName = "Levels/LevelConfig", fileName = "LevelConfig")]
    public class LevelsConfig : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<LevelData> _levels;

        public LevelData GetLevelData(int levelId)
        {
            return _levels.FirstOrDefault(x => x.Id == levelId);
        }
        
        #if UNITY_EDITOR

        public void OnBeforeSerialize()
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                var levelData = _levels[i];
                levelData.Id = i;
                _levels[i] = levelData;
            }
        }

        public void OnAfterDeserialize()
        {
           
        }
        
        #endif
        
     
    }
}
