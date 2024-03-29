using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.UiService.Base
{
    public abstract class BaseUIView : MonoBehaviour
    {
        public virtual void Initialize() { }

        public virtual void Dispose() { }

        public virtual UniTask Show()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public virtual UniTask Hide()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}