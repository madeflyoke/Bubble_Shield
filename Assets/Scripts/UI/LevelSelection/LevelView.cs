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
        
        public void Initialize(Action onClick)
        {
            _button.onClick.AddListener(()=>
            {
                _button.enabled = false;
                onClick?.Invoke();
            });
        }
        
        public void SetLevelTitle(string levelTitle)
        {
            _levelTitle.text = levelTitle;
        }
        
        private void OnEnable()
        {
            _button.enabled = true;
        }
    }
}
