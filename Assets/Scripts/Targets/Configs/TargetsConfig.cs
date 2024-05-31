using System.Collections.Generic;
using System.Linq;
using Targets.Enums;
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
            var configDatas = _targetVariantsData.FirstOrDefault(x => x.Variant == variant).ConfigDatas;
            return configDatas[Random.Range(0, configDatas.Count)].Sprite;
        }

        public Color GetRelatedTargetColor(Sprite sprite)
        {
            TargetConfigData configData = _targetVariantsData
                .SelectMany(data => data.ConfigDatas)
                .FirstOrDefault(config => config.Sprite == sprite);
            return configData.RelatedColor;
        }
    }
}
