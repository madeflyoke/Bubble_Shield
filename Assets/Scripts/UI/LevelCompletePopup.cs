using System.Collections.Generic;
using Managers;
using Score.Utility;
using Services;
using Signals;
using UnityEngine;
using Zenject;

namespace UI
{
    public class LevelCompletePopup : MonoBehaviour
    {
        [Inject] private ServicesHolder _servicesHolder;

        [SerializeField] private List<LevelStar> _levelStars;
        [SerializeField] private BackToLevelSelectorButton _backToSelectorButton;
        [SerializeField] private RestartLevelButton _restartLevelButton;
        [SerializeField] private PopupAnimator _popupAnimator;

        public void Show(LevelCompletedSignal signal)
        {
            _levelStars.ForEach(x=>x.Hide());
            var starsCount = ScoreUtilities.GetStarsCountByScore(signal.LevelData.TargetScore, signal.WrongScoreAnswers,
                _levelStars.Count);
            for (int i = 0; i < starsCount; i++)
            {
                _levelStars[i].Show(0.5f +i*1f);
            }
            
            _backToSelectorButton.Enable(Hide);
            _restartLevelButton.Enable(Hide);
            
            _servicesHolder.GetService<PauseService>().SetPause(true);
            
            _popupAnimator.PlayShowAnimation();
        }
        
        public void Hide()
        {
            _backToSelectorButton.Disable();
            _restartLevelButton.Disable();
            
            _popupAnimator.PlayHideAnimation(() =>
            {
                _servicesHolder.GetService<PauseService>().SetPause(false);
            });
        }
    }
}
