using Cysharp.Threading.Tasks;
using Internal.Scripts.Infrastructure.Constants;
using Internal.Scripts.Infrastructure.GameStatesMachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameBootstrap : MonoBehaviour
{
    private IGameStatesMachine _gameStatesMachine;

    [Inject]
    private void Construct(IGameStatesMachine gameStatesMachine)
    {
        _gameStatesMachine = gameStatesMachine;
    }
    
    private async void Start()
    {
        DontDestroyOnLoad(this);
        
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.MENU_SCENE_NAME);

        while (!loadSceneAsync.isDone)
            await UniTask.Yield();
        
        _gameStatesMachine.Enter();
    }
}
