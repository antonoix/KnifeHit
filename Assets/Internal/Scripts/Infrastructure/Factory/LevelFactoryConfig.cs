using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Internal.Scripts.Infrastructure.Factory
{
    [CreateAssetMenu(fileName = "LevelFactoryConfig", menuName = "Configs/LevelFactoryConfig")]
    public class LevelFactoryConfig : ScriptableObject
    {
        [field: SerializeField] public MainHero MainHero { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject[] LevelContexts { get; private set; }
        [field: SerializeField] public Projectile[] AllProjectiles { get; private set; }
    }
}