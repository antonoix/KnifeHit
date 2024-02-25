using Internal.Scripts.Infrastructure.Injection;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.Sound
{
    [CreateAssetMenu(fileName = "SoundsServiceInjector", menuName = "Injectors/SoundsServiceInjector")]
    public class SoundsServiceInjector : ServiceInjector<ISoundsService>
    {
        [SerializeField] private SoundsConfig config;
        
        public override ISoundsService Create()
        {
            _service = new SoundsService(config);
            return _service;
        }

        public override void Initialize()
        {
            _service.Initialize();
        }
    }
}