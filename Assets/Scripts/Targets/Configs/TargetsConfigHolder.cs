using System.Collections.Generic;
using System.Linq;
using Targets.Enums;
using UnityEngine;
using Utility;

namespace Targets.Configs
{
    public class TargetsConfigHolder : MonoBehaviour
    {
        [SerializeField] private TargetsConfig _targetsConfig;
        
        private Dictionary<Sprite, Color> _spritesPerColorMap;
        private Dictionary<TargetVariant, List<Sprite>> _targetsSpritesMap;
        private Sprite _previousRequestedSprite;
        
        public void Initialize()
        {
            _spritesPerColorMap = _targetsConfig.TargetVariantsData
                .SelectMany(data => data.ConfigDatas).ToDictionary(x => x.Sprite, z => z.RelatedColor);

            _targetsSpritesMap =
                _targetsConfig.TargetVariantsData.ToDictionary(x => x.Variant, z => z.ConfigDatas.Select(x => x.Sprite).ToList());
            _previousRequestedSprite = _targetsSpritesMap[TargetVariant.ENEMY][0]; //default value means nothing
        }

        public Sprite GetRandomSprite(TargetVariant variant)
        {
            var sprites = _targetsSpritesMap[variant];
            if (sprites.Contains(_previousRequestedSprite))
            {
                sprites.ShuffleWithoutLastRepeat(_previousRequestedSprite);
            }
            else
            {
                sprites.Shuffle();
            }
            return sprites[0];
        }

        public Color GetRelatedTargetColor(Sprite sprite)
        {
            return _spritesPerColorMap[sprite];
        }
    }
}
