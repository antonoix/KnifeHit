using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.UiService.Base
{
    public interface IBaseUiView<out T> where T : BaseUIView
    {
        void Initialize(GameObject view);
    }
}