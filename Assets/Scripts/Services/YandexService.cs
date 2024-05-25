using System.Collections;
using Agava.YandexGames;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class YandexService : IService
    {
        private void Awake()
        {
            YandexGamesSdk.CallbackLogging = true;
        }

        private IEnumerator Start()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            yield break;
#endif
            // Always wait for it if invoking something immediately in the first scene.
            yield return YandexGamesSdk.Initialize();

            //if (PlayerAccount.IsAuthorized == false)
               // PlayerAccount.StartAuthorizationPolling(1500);
        }

        public async void Initialize()
        {
            
        }
    }
}
