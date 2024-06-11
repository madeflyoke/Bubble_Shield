using Services;
using Signals;
using UnityEngine;
using Zenject;

namespace UI
{
    public class MatchCompletePopup : MonoBehaviour
    {
        [Inject] private ServicesHolder _servicesHolder;

        public bool IsShowed { get; private set; }

        [SerializeField] private MatchCompleteScoreView _scoreView;
        [SerializeField] private RestartMatchButton _restartMatchButton;
        [SerializeField] private PopupAnimator _popupAnimator;

        public void Show(MatchCompletedSignal signal)
        {
            IsShowed = true;
            
            _scoreView.SetScoreView(signal.FinalScore, _servicesHolder.GetService<ProgressService>().ScoreRecord);
            _restartMatchButton.Enable(Hide);
            _servicesHolder.GetService<PauseService>().SetPause(true);
            _popupAnimator.PlayShowAnimation();
        }
        
        public void Hide()
        {
            _restartMatchButton.Disable();
            
            _popupAnimator.PlayHideAnimation(() =>
            {
                _servicesHolder.GetService<PauseService>().SetPause(false);
            });
            
            IsShowed = false;
        }
    }
}
