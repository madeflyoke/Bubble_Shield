using System;
using Levels.Configs;
using Score.Controller;
using Targets.Managers;
using UnityEngine;

namespace Levels.Managers
{
    public class LevelBootstrapper : MonoBehaviour
    {
        [SerializeField] private TargetsController _targetsController;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private int _id;

        public void Start()
        {
            var levelData = _levelsConfig.GetLevelData(_id);
            _targetsController.Initialize(levelData.TargetsSpawnData, levelData.levelTargetsStats);
            _scoreController.Initialize(levelData.TargetScore);
        }
    }
}
