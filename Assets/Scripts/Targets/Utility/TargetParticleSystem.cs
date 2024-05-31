using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using Lean.Pool;
using UnityEngine;

namespace Targets.Utility
{
    public class TargetParticleSystem : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _rootedParticles;

        public void Play()
        {
            _rootedParticles[0].Play();
        }
        
        public void SetColor(Color color)
        {
            _rootedParticles.ForEach(x=>
            {
                var mainModule = x.main;
                mainModule.startColor = color;
            });
        }

        private void OnParticleSystemStopped()
        {
            LeanPool.Despawn(this);
        }

#if UNITY_EDITOR

        [Button]
        private void SetRootedParticles()
        {
            _rootedParticles = new List<ParticleSystem>();
            _rootedParticles = GetComponentsInChildren<ParticleSystem>().ToList();
        }

#endif
    }
}
