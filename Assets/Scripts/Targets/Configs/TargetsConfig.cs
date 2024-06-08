using System.Collections.Generic;
using UnityEngine;

namespace Targets.Configs
{
    [CreateAssetMenu(menuName = "Targets/Configs", fileName = "Config")]
    public class TargetsConfig : ScriptableObject
    {
        public List<TargetVariantsData> TargetVariantsData => _targetVariantsData;
        
        [SerializeField] private List<TargetVariantsData> _targetVariantsData;
    }
}
