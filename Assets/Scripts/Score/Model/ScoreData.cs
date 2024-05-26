using UnityEngine;
using Utility;

namespace Score.Model
{
    public class ScoreData
    {
        public int CurrentScoreValue => _cachedScoreValue;
        private int _cachedScoreValue;

        public ScoreData()
        {
            _cachedScoreValue = ExtractData();
        }
        
        public void SetScoreValue(int scoreValue ,bool withSave=false)
        {
            _cachedScoreValue = scoreValue;
            if (withSave)
            {
                SaveData();
            }
        }

        public void SaveData()
        {
            PlayerPrefs.SetInt(PlayerPrefsSaveKeys.SCORE_VALUE_KEY, _cachedScoreValue);
        }
        
        private int ExtractData()
        {
            return PlayerPrefs.GetInt(PlayerPrefsSaveKeys.SCORE_VALUE_KEY);
        }
    }
}
