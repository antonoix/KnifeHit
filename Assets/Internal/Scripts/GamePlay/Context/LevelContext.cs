using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.Infrastructure.HeroRoute;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies
{
    public class LevelContext : MonoBehaviour
    {
        [field: SerializeField] public HeroRouter HeroRouter { get; private set; }
        [field: SerializeField] public EnemiesHolder EnemiesHolder { get; private set; }
    }
}
