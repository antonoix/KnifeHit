using Cysharp.Threading.Tasks;
using Internal.Scripts.Infrastructure.Services;
using UnityEngine;

namespace Internal.Scripts.GamePlay.SpecialEffectsService
{
    public interface ISpecialEffectsService : IService
    {
        UniTask ShowEffect(SpecialEffectType type, Vector3 worldPos, Vector3 worldRot);
    }
}