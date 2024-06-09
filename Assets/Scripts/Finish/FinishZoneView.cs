using TMPro;
using UniRx;
using UnityEngine;

namespace Finish
{
    public class FinishZoneView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;
        
        public void LinkReactProperty(IntReactiveProperty health)
        {
            health.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(SetHealthText).AddTo(this);
        }
        
        private void SetHealthText(int value)
        {
            _healthText.text = value.ToString();
        }
    }
}
