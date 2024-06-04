using System.Collections.Generic;
using Levels.Configs;
using Services;
using Signals;
using UnityEngine;
using Zenject;

namespace UI.LevelSelection
{
    public class LevelSelector : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private ServicesHolder _servicesHolder;
        
        [SerializeField] private LevelView _levelViewPrefab;

        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private RectTransform _levelsViewParent;
        private List<LevelView> _levelsViews;

        public void Initialize()
        {
            InitializeLevelViews();
            RefreshLevelViews(_servicesHolder.GetService<ProgressService>().LastCompletedLevel);
            _signalBus.Subscribe<LevelCompletedSignal>(OnLevelCompleted);
        }

        private void OnLevelCompleted(LevelCompletedSignal signal)
        {
            RefreshLevelViews(signal.LevelData.Id);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void RefreshLevelViews(int levelId)
        {
            for (int i = 0, count = _levelsViews.Count; i < count; i++)
            {
                _levelsViews[i].SetOpenedView(i <= levelId+1);
            }
        }
        
        private void InitializeLevelViews()
        {
            string prefix = "LEVEL ";
            
            _levelsViews = new List<LevelView>();
            
            for (int i = 0; i < _levelsConfig.LevelsCount; i++)
            {
                var levelView = Instantiate(_levelViewPrefab, _levelsViewParent);
                _levelsViews.Add(levelView);
                
                int id = i;
                levelView.SetLevelTitle(prefix + (i+1));
                
                _levelsViews[i].Initialize(() =>
                {
                    _signalBus.Fire(new LevelSelectedSignal(id));
                });
            }
        }
    }
}
