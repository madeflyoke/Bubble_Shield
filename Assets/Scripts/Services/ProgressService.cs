using System.Threading;
using Cysharp.Threading.Tasks;
using Services.Interfaces;
using Signals;
using UnityEngine;
using Utility;
using Zenject;

namespace Services
{
    public class ProgressService : IService
    {
        [Inject] private SignalBus _signalBus;

        public int LastCompletedLevel { get; private set; }
        
        public UniTask Initialize(CancellationTokenSource cts)
        {
            ExtractData();
            _signalBus.Subscribe<LevelCompletedSignal>(OnLevelCompleted);
            return UniTask.CompletedTask;
        }

        private void OnLevelCompleted(LevelCompletedSignal signal)
        {
            PlayerPrefs.SetString(PlayerPrefsSaveKeys.LEVEL_COMPLETED_KEY, signal.LevelData.Id.ToString());
        }

        private void ExtractData()
        {
            if (PlayerPrefs.HasKey(PlayerPrefsSaveKeys.LEVEL_COMPLETED_KEY)==false)
            {
                LastCompletedLevel = -1;
            }
            else
            {
                int.TryParse(PlayerPrefs.GetString(PlayerPrefsSaveKeys.LEVEL_COMPLETED_KEY), out int level);
                LastCompletedLevel = level;
            }
        }
    }
}
