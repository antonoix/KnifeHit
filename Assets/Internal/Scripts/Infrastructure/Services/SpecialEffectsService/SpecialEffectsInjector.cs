using Internal.Scripts.Infrastructure.Injection;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.SpecialEffectsService
{
    [CreateAssetMenu(fileName = "SpecialEffectsInjector", menuName = "Injectors/SpecialEffectsInjector")]
    public class SpecialEffectsInjector : ServiceInjector<ISpecialEffectsService>
    {
        [SerializeField] private SpecialEffectsConfig config;

        public override ISpecialEffectsService Create()
        {
            return new SpecialEffectsService(config);
        }

        public override void Initialize()
        {

            _service.Initialize();
        }
    }
}