using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.UI.GamePlay;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies
{
    [CreateAssetMenu(fileName = "GamePlayStateDependency", menuName = "Infrastructure/GamePlayStateDependency")]
    public class GamePlayStateDependency : ScriptableObject, IGameStateDepedency
    {
        [field: SerializeField] public MainHero MainHero { get; private set; }
        [field: SerializeField] public GameplayUIView GameplayUIPrefab { get; private set; }
        [field: SerializeField] public LevelContext[] LevelContexts { get; private set; }
        [field: SerializeField] public SpecialEffectsInjector SpecialEffectsInjector { get; private set; }
    }
}