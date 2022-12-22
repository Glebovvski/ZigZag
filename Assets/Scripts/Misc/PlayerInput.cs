using System;
using Managers;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class PlayerInput : MonoBehaviour
{
    private const string speedKey = "Speed";

    private ScoreManager ScoreManager { get; set; }
    private GameController GameController { get; set; }
    private LevelManager LevelManager { get; set; }

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Transform aiDestination;

    [SerializeField] private float speed;
    public float Speed => speed;

    private Vector3 left = new Vector3(0, 0, 1);
    private Vector3 right = new Vector3(1, 0, 0);

    private Vector3 direction;

    private bool _isLeftDirection = true;

    private bool _isGameStarted = false;

    private bool _isPaused = false;
    public bool IsAIControlled => agent.enabled;

    public event Action<bool> OnPauseToggle;

    [Inject]
    private void Construct(
        ScoreManager scoreManager,
        GameController gameController,
        LevelManager levelManager
    )
    {
        ScoreManager = scoreManager;
        GameController = gameController;
        LevelManager = levelManager;
    }

    public void ToggleAI(bool value)
    {
        agent.enabled = value;
        aiDestination.position = transform.position + new Vector3(15, 0, 15);
    }

    private void Start()
    {
        speed = PlayerPrefs.GetFloat(speedKey, 10);
        direction = left;
        _isGameStarted = false;

        GameController.OnPause += Pause;
        GameController.OnGameStarted += GameStarted;
    }

    private void Update()
    {
        if (!_isGameStarted) return;

        if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) 
        {
            TogglePause();
        }

        if (IsAIControlled)
            AINavigation();
        else
            InputFromPlayer();
    }

    private void GameStarted() => _isGameStarted = !_isGameStarted;

    private void InputFromPlayer()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0) && !_isPaused)
        {
            ScoreManager.UpdateScore(1);
            _isLeftDirection = !_isLeftDirection;
            direction = _isLeftDirection ? left : right;
        }

        var step = speed * Time.deltaTime;
        this.transform.position += direction * step;
    }

    private void AINavigation()
    {
        var destination = LevelManager.NextAIDestiantion + new Vector3(0, 2, 0);
        if (agent.isStopped || Vector3.Distance(this.transform.position, destination) < 3)
        {
            destination = LevelManager.UpdateNextDestination() + new Vector3(0, 2, 0);
        }
        aiDestination.position = destination;
        agent.destination = destination;
    }

    public void Pause() => _isGameStarted = false;

    public void UpdateSpeed(float value)
    {
        speed = value;
        PlayerPrefs.SetFloat(speedKey, value);
    }

    internal void UpdateAIDestination(Vector3 position)
    {
        agent.destination = position;
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0 : 1;
        OnPauseToggle?.Invoke(_isPaused);
    }
    
    private void OnDestroy()
    {
        GameController.OnPause -= Pause;
        GameController.OnGameStarted -= GameStarted;
    }
}
