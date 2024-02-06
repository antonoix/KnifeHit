using Internal.Scripts.Infrastructure.Injection;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.UiService
{
    [CreateAssetMenu(fileName = "UiServiceInjector", menuName = "Injectors/UiServiceInjector")]
    public class UiServiceInjector : ServiceInjector<IUiService>
    {
        [SerializeField] private UiConfig config;

        public override IUiService Create()
        {
            return new UIService(config);
        }

        public override void Initialize()
        {
            _service.Initialize();
        }
    }
}