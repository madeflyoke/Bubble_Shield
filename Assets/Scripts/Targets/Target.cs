using System;
using Targets.Components;
using Targets.Enums;
using Targets.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Targets
{
    public class Target : MonoBehaviour
    {
        public static event Action<Target> TargetClicked;

        public TargetVariant Variant => _targetData.Variant;
        
        [field: SerializeField] public Collider2D Collider { get; private set; }
        
        [SerializeField] private TargetViewComponent _viewComponent;
        [SerializeField] private TargetEffectsComponent _effectComponent;

        private TargetData _targetData;
        private float _currentSpeed;
        
        public void Initialize(TargetData targetData)
        {
            _targetData = targetData;
            
            _effectComponent.Initialize(targetData.RelatedColor);
            _viewComponent.Initialize(_targetData.Sprite, ()=>
            {
                _effectComponent.PlayOnDeathParticle();
                TargetClicked?.Invoke(this);
            });
            
            _currentSpeed = targetData.Stats.Speed * Random.Range(0.9f,1.3f); //move out range to config (worth it?)
        }
        
        private void FixedUpdate()
        {
            transform.position -= transform.up * Time.fixedDeltaTime * _currentSpeed;
        }
    }
}
