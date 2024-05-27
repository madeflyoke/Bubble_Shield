using System;
using Targets.Enums;
using Targets.Utility;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Targets
{
    public class Target : MonoBehaviour
    {
        public static event Action<Target> TargetClicked;

        public TargetVariant Variant => _targetData.Variant;
        [field: SerializeField] public Collider2D Collider { get; private set; }
        
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        
        private TargetData _targetData;
        private float _currentSpeed;

        public void Initialize(TargetData targetData)
        {
            _targetData = targetData;
            _image.preserveAspect = true;
            SetSprite(_targetData.Sprite);
            _button.onClick.AddListener(OnButtonClicked);
            _currentSpeed = targetData.Stats.Speed * Random.Range(0.9f,1.3f);
        }

        private void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        private void FixedUpdate()
        {
            transform.position -= transform.up * Time.fixedDeltaTime * _currentSpeed;
        }

        private void OnButtonClicked()
        {
            TargetClicked?.Invoke(this);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}
