using Internal.Scripts.Infrastructure.Services.Base;
using Internal.Scripts.Infrastructure.Services.UiService.Base;

namespace Internal.Scripts.Infrastructure.Services.UiService
{
    public interface IUiService : IService
    {
        void Initialize();

        IBaseUiView<BaseUIView> GetPresenter<T>() where T : IBaseUiView<BaseUIView>;
    }
}