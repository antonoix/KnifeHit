using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Internal.Scripts.GamePlay.TheMainHero
{
    [CreateAssetMenu(fileName = "MainHeroConfig", menuName = "Configs/MainHeroConfig")]
    public class MainHeroConfig : ScriptableObject
    {
        [field: SerializeField] public AssetReference MainHeroReference { get; private set; }
        [field: SerializeField] public float RotationDurationSec { get; private set; } = 0.7f;
        [field: SerializeField] public float ShootDelaySec { get; private set; } = 0.3f;
        [field: SerializeField] public float TimeBetweenRotationCheck { get; private set; } = 0.4f;
        [field: SerializeField] public LayerMask AimLayers { get; private set; }
    }
}