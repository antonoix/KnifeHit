using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies;
using Internal.Scripts.Infrastructure.Services.SpecialEffectsService;
using Internal.Scripts.Infrastructure.Services.UiService;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Injection.StatesDependencies
{
    [CreateAssetMenu(fileName = "GamePlayStateDependency", menuName = "Infrastructure/GamePlayStateDependency")]
    public class GamePlayStateDependency : ScriptableObject
    {
        [field: SerializeField] public MainHero MainHero { get; private set; }
        [field: SerializeField] public LevelContext[] LevelContexts { get; private set; }
        [field: SerializeField] public SpecialEffectsInjector SpecialEffectsInjector { get; private set; }
        [field: SerializeField] public UiServiceInjector UiServiceInjector { get; private set; }
    }
}