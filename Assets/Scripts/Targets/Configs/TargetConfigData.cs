using System;
using Attributes;
using UnityEngine;

namespace Targets.Configs
{
    [Serializable]
    public class TargetConfigData
    {
        [PreviewSprite] public Sprite Sprite;
        public Color RelatedColor = new Color(0,0,0,1);
    }
}
