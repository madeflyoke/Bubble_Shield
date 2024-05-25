using System;
using Managers;
using Targets;
using Targets.Enums;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TargetsController _targetsController;

        private void OnEnable()
        {
            _targetsController.TargetFinished += OnTargetFinished;
        }
        private void OnDisable()
        {
            _targetsController.TargetFinished -= OnTargetFinished;
        }
        
        private void OnTargetFinished(Target target)
        {
            switch (target.Variant)
            {
                case TargetVariant.ENEMY:
                    break;
                case TargetVariant.ALLY:
                    break;
            }
        }

    
    }
}
