using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Internal.Scripts.GamePlay.ProjectilesService
{
    public interface IProjectilesService
    {
        void ThrowProjectile(Vector3 startPoint, Vector3 destinationPoint);
    }
}