using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LevelSelection
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _levelTitle;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private Color _lockedButtonColor;
        [SerializeField] private Image _lockImage;
        private Color _defaultButtonColor;
        
        public void Initialize(Action onClick)
        {
            _defaultButtonColor = _buttonImage.color;
            _button.onClick.AddListener(()=>
            {
                onClick?.Invoke();
            });
        }
        
        public void SetLevelTitle(string levelTitle)
        {
            _levelTitle.text = levelTitle;
        }
        
        public void SetOpenedView(bool isOpened)
        {
            _levelTitle.gameObject.SetActive(isOpened);
            _lockImage.gameObject.SetActive(!isOpened);
            _buttonImage.color = isOpened? _defaultButtonColor : _lockedButtonColor;
            _button.enabled = isOpened;
        }
    }
}
