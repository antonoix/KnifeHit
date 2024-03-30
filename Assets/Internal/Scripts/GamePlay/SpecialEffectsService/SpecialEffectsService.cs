using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.GamePlay.SpecialEffectsService
{
    public class SpecialEffectsService : ISpecialEffectsService, IInitializable
    {
        private const int TO_MS = 1000;
        private readonly SpecialEffectsConfig _config;
        private readonly Dictionary<SpecialEffectType, Stack<ParticleSystem>> _specialEffects = new();

        public SpecialEffectsService(SpecialEffectsConfig config)
        {
            _config = config;
        }

        public void Initialize()
        {
            foreach (var specialEffect in _config.AllSpecialEffects)
            {
                if (_specialEffects.ContainsKey(specialEffect.Type))
                {
                    Debug.LogError("{specialEffect.Type} already exists");
                    continue;
                }
                _specialEffects.Add(specialEffect.Type, new Stack<ParticleSystem>());
            }
        }

        public async UniTask ShowEffect(SpecialEffectType type, Vector3 worldPos)
        {
            if (!TryGetEffect(type, out var effect)) return;

            effect.transform.position = worldPos;
            effect.gameObject.SetActive(true);
            effect.Play();
            await UniTask.Delay((int)(effect.main.duration * TO_MS));
            effect.gameObject.SetActive(false);
            _specialEffects[type].Push(effect);
        }

        public void Dispose()
        {
            _specialEffects.Clear();
        }

        private bool TryGetEffect(SpecialEffectType type, out ParticleSystem effect)
        {
            effect = null;
            if (_specialEffects[type].Count > 0)
            {
                effect = _specialEffects[type].Pop();
            }
            else
            {
                var prefab = _config.AllSpecialEffects.FirstOrDefault(x => x.Type == type);
                if (prefab == null)
                {
                    Debug.LogError($"Can not find type in config: {type}");
                    return false;
                }

                effect = GameObject.Instantiate(prefab.SpecialEffectPrefab);
                GameObject.DontDestroyOnLoad(effect);
            }

            return true;
        }
    }
}