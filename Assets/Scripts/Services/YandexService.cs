using System.Threading;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Services.Interfaces;

namespace Services
{
    public class YandexService : IService
    {
        
        public async UniTask Initialize(CancellationTokenSource cts)
        {
            YandexGamesSdk.CallbackLogging = true;
            
#if !UNITY_WEBGL || UNITY_EDITOR
            await UniTask.Delay(1000, cancellationToken:cts.Token).SuppressCancellationThrow();
#else
            await YandexGamesSdk.Initialize().ToUniTask(cancellationToken:cts.Token).SuppressCancellationThrow();
            
            //if (PlayerAccount.IsAuthorized == false)
            // PlayerAccount.StartAuthorizationPolling(1500);
#endif
            
        }
    }
}
