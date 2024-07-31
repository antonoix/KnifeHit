using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Weapons.ProjectilesService
{
    public interface IProjectilesService
    {
        UniTaskVoid ThrowProjectile(Vector3 startPoint, Vector3 destinationPoint);
    }
}