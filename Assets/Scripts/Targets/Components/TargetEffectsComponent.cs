using Lean.Pool;
using Targets.Utility;
using UnityEngine;

namespace Targets.Components
{
    public class TargetEffectsComponent : MonoBehaviour
    {
        private const float ZPos = 40f; //far than camera closer than canvas
        
        [SerializeField] private TargetParticleSystem _onDeathParticlePrefab; //shared among many targets, be sure to not change it directly
        private Color _relatedColor;
        
        public void Initialize(Color relatedColor)
        {
            _relatedColor = relatedColor;
        }

        public void PlayOnDeathParticle()
        {
            var spawnPosition = transform.position;
            spawnPosition.z = ZPos;
            var effect = LeanPool.Spawn(_onDeathParticlePrefab, spawnPosition, Quaternion.identity);
            effect.SetColor(_relatedColor);
            effect.Play();
        }
    }
}
