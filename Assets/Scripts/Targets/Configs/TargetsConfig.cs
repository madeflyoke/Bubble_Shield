using System.Collections.Generic;
using System.Linq;
using Targets.Enums;
using Targets.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Targets.Configs
{
    [CreateAssetMenu(menuName = "Targets/Configs", fileName = "Config")]
    public class TargetsConfig : ScriptableObject
    {
        [SerializeField] private List<TargetVariantsData> _targetVariantsData;

        public Sprite GetRandomSprite(TargetVariant variant)
        {
            var sprites = _targetVariantsData.FirstOrDefault(x => x.Variant == variant).Sprites;
            return sprites[Random.Range(0, sprites.Count)];
        }
    }
}
