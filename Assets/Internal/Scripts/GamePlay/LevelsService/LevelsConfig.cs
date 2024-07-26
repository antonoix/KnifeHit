using System;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Internal.Scripts.Infrastructure.Factory
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Configs/LevelsConfig")]
    public class LevelsConfig : ScriptableObject
    {
        [field: SerializeField] public AssetLabelReference LevelLabel { get; private set; }
        [field: SerializeField] public WeaponProjectile[] AllProjectiles { get; private set; }
    }
}