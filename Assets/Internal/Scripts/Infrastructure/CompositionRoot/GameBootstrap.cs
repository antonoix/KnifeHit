using System;
using Internal.Scripts.Infrastructure.GameStatesMachine;
using Internal.Scripts.Infrastructure.GameStatesMachine.Injection;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private GameStatesMachineDependencies gameStatesMachineDependencies;
    
    private GameStatesMachine _gameStatesMachine;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _gameStatesMachine = new GameStatesMachine(gameStatesMachineDependencies);
        _gameStatesMachine.Enter();
    }

    private void OnDestroy()
    {
        _gameStatesMachine.Dispose();
    }
}
