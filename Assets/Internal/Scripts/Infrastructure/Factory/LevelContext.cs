using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.GamePlay.HeroRoute;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Factory
{
    public class LevelContext : MonoBehaviour
    {
        [field: SerializeField] public HeroRouter HeroRouter { get; private set; }
        [field: SerializeField] public EnemiesHolder EnemiesHolder { get; private set; }
    }
}
