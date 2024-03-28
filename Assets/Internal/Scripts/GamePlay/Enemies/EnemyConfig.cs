using UnityEngine;

namespace Internal.Scripts.GamePlay.Enemies
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [field: SerializeField] public float SpeedMeterPerSec { get; private set; } = 0.75f;
        [field: SerializeField] public float AttackDistance { get; private set; } = 1;
        [field: SerializeField] public float DisableTimeAfterDamagedSec { get; private set; } = 3f;
        [field: SerializeField] public float StandingUpTimeAfterDamagedSec { get; private set; } = 1.3f;
        [field: SerializeField] public int Health { get; private set; } = 3;
        [field: SerializeField] public int RewardCoinsCount { get; private set; }
    }
}