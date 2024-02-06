using Internal.Scripts.Infrastructure.GameStatesMachine;
using Internal.Scripts.Infrastructure.Injection;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private ProjectDependencies projectDependencies;
    
    private GameStatesMachine _gameStatesMachine;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _gameStatesMachine = new GameStatesMachine(projectDependencies);
        _gameStatesMachine.Enter();
    }

    private void OnDestroy()
    {
        _gameStatesMachine.Dispose();
    }
}
