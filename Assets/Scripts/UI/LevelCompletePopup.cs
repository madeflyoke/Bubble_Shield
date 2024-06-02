using System.Collections.Generic;
using Managers;
using Score.Utility;
using Services;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LevelCompletePopup : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private ServicesHolder _servicesHolder;

        [SerializeField] private List<LevelStar> _levelStars;
        [SerializeField] private BackToLevelSelectorButton _backToSelectorButton;
        [SerializeField] private Button _restartLevelButton;

        public void Show(LevelCompletedSignal signal)
        {
            _levelStars.ForEach(x=>x.SetActive(false));
            var starsCount = ScoreUtilities.GetStarsCountByScore(signal.LevelData.TargetScore, signal.WrongScoreAnswers,
                _levelStars.Count);
            for (int i = 0; i < starsCount; i++)
            {
                _levelStars[i].SetActive(true);
            }
            
            _backToSelectorButton.Enable(Hide);
            _restartLevelButton.onClick.AddListener(RestartLevel);
            _servicesHolder.GetService<PauseService>().SetPause(true);
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
            _servicesHolder.GetService<PauseService>().SetPause(false);
            _backToSelectorButton.Disable();
            _restartLevelButton.onClick.RemoveListener(RestartLevel);
        }
        
        private void RestartLevel()
        {
            _signalBus.Fire<LevelResetSignal>();
            _signalBus.Fire<CallOnRestartLevel>();
            Hide();
        }
    }
}
