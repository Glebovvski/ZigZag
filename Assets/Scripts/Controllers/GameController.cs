using System;
using Managers;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    private LevelManager LevelManager { get; set; }
    private PlayerInput PlayerInput { get; set; }
    private const string gamesPlayedKey = "GamesPlayed";

    [SerializeField] private GameObject ballStartPos;

    [SerializeField] private Transform _ball;

    [SerializeField] private Transform startPlane;

    public event Action OnLose;
    public event Action OnRetry;
    public event Action OnPause;
    public event Action OnOpenSettings;
    public event Action OnCloseSettings;
    public event Action OnGameStarted;

    public int GamesPlayed => PlayerPrefs.GetInt(gamesPlayedKey, 0);

    [Inject]
    private void Construct(LevelManager levelManager, PlayerInput playerInput)
    {
        LevelManager = levelManager;
        PlayerInput = playerInput;
    }

    public void Retry()
    {
        OnRetry?.Invoke();
        startPlane.position = Vector3.zero;
        LevelManager.InitLevel();
        _ball.transform.position = ballStartPos.transform.position;
    }

    public void Pause() => OnPause?.Invoke();

    public void Lose()
    {
        OnLose?.Invoke();
        int gamesPlayed = GamesPlayed + 1;
        PlayerPrefs.SetInt(gamesPlayedKey, gamesPlayed);
    }

    public void OpenSettings() => OnOpenSettings?.Invoke();

    public void CloseSettings()
    {
        OnCloseSettings?.Invoke();
        Retry();
    }

    public void TogglePause()
    {
        PlayerInput.TogglePause();
    }

    public void StartGame() => OnGameStarted?.Invoke();
}
