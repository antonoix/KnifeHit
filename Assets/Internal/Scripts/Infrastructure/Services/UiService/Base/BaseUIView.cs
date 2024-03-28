using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.UiService.Base
{
    public abstract class BaseUIView : MonoBehaviour
    {
        public virtual void Initialize() { }

        public virtual void Dispose() { }

        public virtual void Show()
            => gameObject.SetActive(true);
        
        public virtual void Hide()
            => gameObject.SetActive(false);
    }
}