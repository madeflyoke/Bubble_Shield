using System.Threading;
using Cysharp.Threading.Tasks;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class PauseService : IService
    {
        public void SetPause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0 : 1;
        }

        public UniTask Initialize(CancellationTokenSource cts)
        {
            return UniTask.CompletedTask;
        }
    }
}
