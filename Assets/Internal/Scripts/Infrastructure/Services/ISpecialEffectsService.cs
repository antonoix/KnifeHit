using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies
{
    public interface ISpecialEffectsService
    {
        UniTask ShowEffect(SpecialEffectType type, Vector3 worldPos);
        void Initialize();
    }
}