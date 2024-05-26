using System.Threading;
using Cysharp.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IService
    {
        public UniTask Initialize(CancellationTokenSource cts);
    }
}
