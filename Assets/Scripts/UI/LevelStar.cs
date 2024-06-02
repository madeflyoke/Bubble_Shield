using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelStar : MonoBehaviour
    {
        [SerializeField] private Image _onImage;

        public void SetActive(bool isActive)
        {
            _onImage.gameObject.SetActive(isActive);
        }
    }
}