using System.Threading;
using Cysharp.Threading.Tasks;
using Score.Controller;
using Services.Interfaces;
using Signals;
using UnityEngine;
using Utility;
using Zenject;

namespace Services
{
    public class ProgressService : IService
    {
        public int ScoreRecord { get; private set; }
        
        public UniTask Initialize(CancellationTokenSource cts)
        {
            ExtractData();
            return UniTask.CompletedTask;
        }

        public void TryUpdateRecord(int currentScore)
        {
            if (currentScore > ScoreRecord)
            {
                PlayerPrefs.SetInt(PlayerPrefsSaveKeys.SCORE_RECORD_KEY, currentScore);
                PlayerPrefs.Save();
                ScoreRecord = currentScore;
            }
        }

        private void ExtractData() //TODO Change to yandex prefs
        {
            ScoreRecord = PlayerPrefs.GetInt(PlayerPrefsSaveKeys.SCORE_RECORD_KEY);
        }
    }
}
