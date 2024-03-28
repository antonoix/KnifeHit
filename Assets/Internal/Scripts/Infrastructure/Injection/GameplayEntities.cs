using Internal.Scripts.GamePlay.Context;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Injection
{
    [CreateAssetMenu(fileName = "GameplayEntities", menuName = "Infrastructure/GameplayEntities")]
    public class GameplayEntities : ScriptableObject
    {
        [field: SerializeField] public MainHero MainHero { get; private set; }
        [field: SerializeField] public LevelContext[] LevelContexts { get; private set; }
        [field: SerializeField] public Projectile[] AllProjectiles { get; private set; }
    }
}