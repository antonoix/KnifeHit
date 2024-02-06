using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.UiService.Base
{
    public abstract class BaseUIView : MonoBehaviour
    {
        public abstract void Initialize();
        public abstract void Dispose();

        public virtual void Show()
            => gameObject.SetActive(true);
        
        public virtual void Hide()
            => gameObject.SetActive(false);
    }
}