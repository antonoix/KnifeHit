using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero
{
    [CreateAssetMenu(fileName = "MainHeroConfig", menuName = "Configs/MainHeroConfig")]
    public class MainHeroConfig : ScriptableObject
    {
        [field: SerializeField] public float RotationDurationSec = 0.7f;
        [field: SerializeField] public float ShootDelaySec = 0.3f;
    }
}