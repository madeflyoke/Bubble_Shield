using System;
using UnityEngine;
using UnityEngine.UI;

namespace Targets.Components
{
    public class TargetViewComponent : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        
        public void Initialize(Sprite sprite, Action onButtonClicked)
        {
            SetSprite(sprite);
            _button.onClick.AddListener(()=>
            {
                onButtonClicked();
                _button.enabled = false;
                _button.onClick.RemoveAllListeners();
            });

            _button.enabled = true;
        }
        
        private void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
