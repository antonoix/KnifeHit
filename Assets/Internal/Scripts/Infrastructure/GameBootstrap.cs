using Internal.Scripts.Infrastructure.GameStatesMachine;
using Internal.Scripts.Infrastructure.GameStatesMachine.Injection;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private GameStatesMachineDependencies gameStatesMachineDependencies;
    
    private GameStatesMachine _gameStatesMachine;
    
    private void Awake()
    {
        _gameStatesMachine = new GameStatesMachine(gameStatesMachineDependencies);
        _gameStatesMachine.Enter();
    }
}
