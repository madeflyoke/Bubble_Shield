using System;
using System.Collections.Generic;
using Targets.Enums;

namespace Targets.Configs
{
    [Serializable]
    public struct TargetVariantsData
    {
        public TargetVariant Variant;
        public List<TargetConfigData> ConfigDatas;
    }
}