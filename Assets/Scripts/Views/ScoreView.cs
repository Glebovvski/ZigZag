using Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views
{
    public class ScoreView : MonoBehaviour
    {
        private ScoreManager ScoreManager { get; set; }
        private PlayerInput PlayerInput { get; set; }
        private GameController GameController { get; set; }

        [SerializeField] private GameObject _scorePanel;

        [SerializeField] private Text _score;

        [Inject]
        private void Construct(ScoreManager scoreManager, PlayerInput playerInput, GameController gameController)
        {
            ScoreManager = scoreManager;
            PlayerInput = playerInput;
            GameController = gameController;
        }

        private void Start()
        {
            ScoreManager.OnScoreChanged += UpdateScore;
            GameController.OnLose += Close;
            GameController.OnGameStarted += Open;
        }

        public void Open() => _scorePanel.SetActive(true);

        public void Close() => _scorePanel.SetActive(false);

        private void UpdateScore(int value) => _score.text = value.ToString();

        private void OnDestroy()
        {
            ScoreManager.OnScoreChanged -= UpdateScore;
            GameController.OnLose -= Close;
            GameController.OnGameStarted -= Open;
        }
    }
}
