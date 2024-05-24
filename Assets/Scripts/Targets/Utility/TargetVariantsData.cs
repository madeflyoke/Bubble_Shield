using System;
using System.Collections.Generic;
using Targets.Enums;
using UnityEngine;

namespace Targets.Utility
{
    [Serializable]
    public struct TargetVariantsData
    {
        public TargetVariant Variant;
        public List<Sprite> Sprites;
    }
}