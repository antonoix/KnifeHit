using Cysharp.Threading.Tasks;
using Internal.Scripts.Infrastructure.Constants;
using Internal.Scripts.Infrastructure.GameStatesMachine;
using Internal.Scripts.Infrastructure.SaveLoad;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameBootstrap : MonoBehaviour
{
    private IGameStatesMachine _gameStatesMachine;
    private IPersistentProgressService _persistentProgressService;
    private ISaveLoadService _saveLoadService;

    [Inject]
    private void Construct(IGameStatesMachine gameStatesMachine,
        IPersistentProgressService persistentProgressService,
        ISaveLoadService saveLoadService)
    {
        _gameStatesMachine = gameStatesMachine;
        _persistentProgressService = persistentProgressService;
        _saveLoadService = saveLoadService;
    }
    
    private async void Start()
    {
        DontDestroyOnLoad(this);
        
        LoadProgress();
        
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.MENU_SCENE_NAME);

        while (!loadSceneAsync.isDone)
            await UniTask.Yield();
        
        _gameStatesMachine.Enter();
    }
    
    private void LoadProgress()
    {
        PlayerProgress playerProgress = _saveLoadService.LoadProgress();
        if (playerProgress == null)
        {
            Debug.Log("PlayerProgress is null, Init New Progress");
            _persistentProgressService.InitNewProgress();
            return;
        }

        Debug.Log("PlayerProgress is loaded");
        _persistentProgressService.PlayerProgress = playerProgress;
    }
}
