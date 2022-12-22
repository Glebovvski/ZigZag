using System;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private GameController GameController {get;set;}

        private int _score = 0;

        public int Score
        {
            get { return _score; }
            private set { _score = value; }
        }
        public int BestScore
        {
            get { return PlayerPrefs.GetInt("BestScore", 0); }
            private set { PlayerPrefs.SetInt("BestScore", value); }
        }

        public event Action<int> OnScoreChanged;

        [Inject]
        private void Construct(GameController gameController)
        {
            GameController = gameController;
        }

        private void Start()
        {
            GameController.OnRetry += ResetScore;
            ResetScore();
        }

        private void ResetScore() 
        {
            Score = 0;
            OnScoreChanged?.Invoke(Score);
        }
        public void UpdateScore(int value)
        {
            Score += value;
            if (Score > BestScore)
            {
                BestScore = Score;
                UpdateBestScore();
            }

            OnScoreChanged?.Invoke(Score);
        }

        private void UpdateBestScore() => PlayerPrefs.SetInt("BestScore", BestScore);

        private void OnDestroy()
        {
            GameController.OnRetry -= ResetScore;
        }
    }
}
