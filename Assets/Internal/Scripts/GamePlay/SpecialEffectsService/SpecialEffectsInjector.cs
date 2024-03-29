using Internal.Scripts.Infrastructure.Injection;
using UnityEngine;

namespace Internal.Scripts.GamePlay.SpecialEffectsService
{
    [CreateAssetMenu(fileName = "SpecialEffectsInjector", menuName = "Injectors/SpecialEffectsInjector")]
    public class SpecialEffectsInjector : ServiceInjector<ISpecialEffectsService>
    {
        [SerializeField] private SpecialEffectsConfig config;

        public override ISpecialEffectsService Create()
        {
            _service = new SpecialEffectsService(config);
            return _service;
        }

        public override void Initialize()
        {
            base.Initialize();
            _service.Initialize();
        }
    }
}