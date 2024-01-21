using Internal.Scripts.Infrastructure.Services;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies
{
    [CreateAssetMenu(fileName = "SpecialEffectsInjector", menuName = "Injectors/SpecialEffectsInjector")]
    public class SpecialEffectsInjector : ScriptableObject
    {
        [SerializeField] private SpecialEffectsConfig config;

        private ISpecialEffectsService _specialEffectsService;

        public ISpecialEffectsService GetService()
        {
            if (_specialEffectsService != null)
                return _specialEffectsService;
            
            _specialEffectsService =  new SpecialEffectsService(config);
            _specialEffectsService.Initialize();
            return _specialEffectsService;
        }
    }
}