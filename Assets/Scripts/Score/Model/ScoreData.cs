using UnityEngine;
using Utility;

namespace Score.Model
{
    public class ScoreData
    {
        public int RecordScoreValue => _recordScoreValue;
        private int _recordScoreValue;

        public ScoreData()
        {
            ExtractData();
        }
        
        private void ExtractData()
        {
            _recordScoreValue = PlayerPrefs.GetInt(PlayerPrefsSaveKeys.RECORD_SCORE_VALUE_KEY,0);
        }
        
        public void SetRecordScoreValue(int value ,bool withSave=false)
        {
            _recordScoreValue = value;
            if (withSave)
            {
                SaveData();
            }
        }

        public void SaveData()
        {
            PlayerPrefs.SetInt(PlayerPrefsSaveKeys.RECORD_SCORE_VALUE_KEY, _recordScoreValue);
        }
    }
}
