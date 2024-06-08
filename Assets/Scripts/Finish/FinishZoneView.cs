using TMPro;
using UnityEngine;

namespace Finish
{
    public class FinishZoneView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;
        
        public void SetHealthText(int value)
        {
            _healthText.text = value.ToString();
        }
    }
}
