using Internal.Scripts.Infrastructure.Services.UiService.Base;

namespace Internal.Scripts.Infrastructure.Services.UiService
{
    public interface IUiService : IService
    {
        IBaseUiView<BaseUIView> GetPresenter<T>() where T : IBaseUiView<BaseUIView>;
    }
}