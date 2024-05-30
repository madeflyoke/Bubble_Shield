using System;
using System.Collections.Generic;
using EasyButtons;
using Lean.Pool;
using Levels.Configs;
using Signals;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace UI.LevelSelection
{
    public class LevelSelector : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private LevelView _levelViewPrefab;

        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private RectTransform _levelsViewParent;
        private List<LevelView> _levelsViews;

        public void Initialize()
        {
            SetupLevelViews();

            for (int i = 0, count = _levelsViews.Count; i < count; i++)
            {
                int id = i;
                _levelsViews[i].Initialize(() =>
                {
                    _signalBus.Fire(new LevelSelectedSignal(id));
                });
            }
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        private void SetupLevelViews()
        {
            string prefix = "LEVEL_";
            
            _levelsViews = new List<LevelView>();
            
            for (int i = 0; i < _levelsConfig.LevelsCount; i++)
            {
                var levelView = Instantiate(_levelViewPrefab, _levelsViewParent);
                _levelsViews.Add(levelView);
                levelView.SetLevelTitle(prefix + i);
            }
        }
    }
}
