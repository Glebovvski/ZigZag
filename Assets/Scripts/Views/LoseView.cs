using Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views
{
    public class LoseView : MonoBehaviour
    {
        private ScoreManager ScoreManager { get; set; }

        private GameController GameController { get; set; }

        [SerializeField] private GameObject losePanel;

        [SerializeField] private Text score;

        [SerializeField] private Text bestScore;

        [Inject]
        private void Construct(ScoreManager scoreManager, GameController gameController)
        {
            ScoreManager = scoreManager;
            GameController = gameController;
        }

        private void Start()
        {
            GameController.OnLose += Open;
            GameController.OnRetry += Close;
        }

        public void Open()
        {
            losePanel.SetActive(true);
            UpdateData();
        }

        public void Close() => losePanel.SetActive(false);

        private void UpdateData()
        {
            score.text = string.Format("Score: {0}", ScoreManager.Score.ToString());
            bestScore.text = string.Format("Best Score: {0}", ScoreManager.BestScore.ToString());
        }

        private void OnDestroy()
        {
            GameController.OnLose -= Open;
            GameController.OnRetry -= Close;
        }
    }
}
