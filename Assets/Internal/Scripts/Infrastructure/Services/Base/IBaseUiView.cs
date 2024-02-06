using Internal.Scripts.Infrastructure.Services.UiService.Base;

namespace Internal.Scripts.Infrastructure.Services.Base
{
    public interface IBaseUiView<out T> where T : BaseUIView
    {
        void Initialize();
    }
}