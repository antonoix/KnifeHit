using System.Collections.Generic;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.Sound
{
    public class SoundsService : ISoundsService
    {
        private readonly SoundsConfig _config;
        private readonly Dictionary<SoundType, AudioSource> _soundTypeAndSources = new();

        public SoundsService(SoundsConfig config)
        {
            _config = config;
        }
        
        public void Initialize()
        {
            var root = Object.Instantiate(new GameObject("Sound"));
            Object.DontDestroyOnLoad(root);

            foreach (var audioSet in _config.AudioSets)
            {
                var soundObject = GameObject.Instantiate(new GameObject(audioSet.Type.ToString()), root.transform);
                var source = soundObject.AddComponent<AudioSource>();
                source.clip = audioSet.Clip;
                source.volume = audioSet.Volume;

                if (!_soundTypeAndSources.TryAdd(audioSet.Type, source))
                    Debug.LogError($"Can not add {audioSet.Type}");
            }
        }

        public void PlaySound(SoundType soundType)
        {
            if (!_soundTypeAndSources.ContainsKey(soundType))
            {
                Debug.LogError($"No {soundType} in dictionary");
                return;
            }

            _soundTypeAndSources[soundType].Play();
        }
    }
}