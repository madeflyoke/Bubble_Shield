using UnityEngine;

namespace Managers
{
    public class PauseManager : MonoBehaviour
    {
        public void SetPause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0 : 1;
        }
    }
}
