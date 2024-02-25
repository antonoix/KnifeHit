using System;
using System.Collections.Generic;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.Sound
{
    [CreateAssetMenu(fileName = "SoundsConfig", menuName = "Configs/SoundsConfig")]
    public class SoundsConfig : ScriptableObject
    {
        [field: SerializeField] public List<AudioSet> AudioSets { get; private set; }
    }

    [Serializable]
    public class AudioSet
    {
        public SoundType Type;
        public AudioClip Clip;
        public float Volume;
    }
}