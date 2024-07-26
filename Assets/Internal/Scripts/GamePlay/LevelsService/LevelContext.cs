using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.GamePlay.HeroRoute;
using Unity.AI.Navigation;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Factory
{
    public class LevelContext : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface surface;
        [field: SerializeField] public HeroRouter HeroRouter { get; private set; }
        [field: SerializeField] public EnemiesHolder EnemiesHolder { get; private set; }
        public int AgentTypeId => surface.agentTypeID;
    }
}
