using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.SpecialEffectsService
{
    public interface ISpecialEffectsService : IService
    {
        UniTask ShowEffect(SpecialEffectType type, Vector3 worldPos);
        void Initialize();
    }
}