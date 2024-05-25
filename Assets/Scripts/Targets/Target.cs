using System;
using Targets.Enums;
using Targets.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Targets
{
    public class Target : MonoBehaviour
    {
        public event Action TargetClicked;

        public TargetVariant Variant => _targetData.Variant;
        [field: SerializeField] public Collider2D Collider { get; private set; }
        
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        
        private TargetData _targetData;

        public void Initialize(TargetData targetData)
        {
            _targetData = targetData;
            _image.preserveAspect = true;
            SetSprite(_targetData.Sprite);
        }

        private void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        private void FixedUpdate()
        {
            transform.position -= transform.up * Time.fixedDeltaTime * 3f;
        }
    }
}
