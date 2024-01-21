using System;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies
{
    [CreateAssetMenu(fileName = "SpecialEffectsConfig", menuName = "Configs/SpecialEffectsConfig")]
    public class SpecialEffectsConfig : ScriptableObject
    {
        [field: SerializeField] public SpecialEffect[] AllSpecialEffects;
        
        [Serializable]
        public class SpecialEffect
        {
            [field: SerializeField] public SpecialEffectType Type { get; private set; }
            [field: SerializeField] public ParticleSystem SpecialEffectPrefab { get; private set; }
        }
    }
}