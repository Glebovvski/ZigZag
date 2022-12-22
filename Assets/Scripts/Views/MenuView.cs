using Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views
{
    public class MenuView : MonoBehaviour
    {
        private ScoreManager ScoreManager { get; set; }
        private PlayerInput PlayerInput { get; set; }
        private GameController GameController { get; set; }
        private CollectibleManager CollectibleManager { get; set; }

        [SerializeField] private GameObject _menuPanel;

        [SerializeField] private Text bestScore;

        [SerializeField] private Text gamesPlayed;

        [SerializeField] private Text diamondScore;

        [Inject]
        private void Construct(
            ScoreManager scoreManager,
            PlayerInput playerInput,
            GameController gameController,
            CollectibleManager collectibleManager
        )
        {
            ScoreManager = scoreManager;
            PlayerInput = playerInput;
            GameController = gameController;
            CollectibleManager = collectibleManager;
        }

        private void Start()
        {
            GameController.OnGameStarted += Close;
            GameController.OnOpenSettings += Close;
            GameController.OnRetry += Open;
            Open();
        }

        public void Open()
        {
            GameController.Pause();
            _menuPanel.SetActive(true);
            UpdateData();
        }

        private void UpdateData()
        {
            bestScore.text = ScoreManager.BestScore.ToString();
            gamesPlayed.text = GameController.GamesPlayed.ToString();
            diamondScore.text = CollectibleManager.Collected.ToString();
        }

        public void Close() => _menuPanel.SetActive(false);

        private void OnDestroy()
        {
            GameController.OnGameStarted -= Close;
            GameController.OnOpenSettings -= Close;
            GameController.OnRetry -= Open;
        }
    }
}
