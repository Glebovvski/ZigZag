using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class CollectibleManager : MonoBehaviour
    {
        private const string DiamondsCollectedKey = "DiamondsCollected";
        private ScoreManager ScoreManager { get; set; }
        private GameController GameController { get; set; }

        [SerializeField] private Collectible _collectiblePrefab;

        private List<Collectible> collectibles;

        public event Action OnUpdateCollectiblesScore;

        public int Collected
        {
            get { return PlayerPrefs.GetInt(DiamondsCollectedKey, 0); }
            private set { PlayerPrefs.SetInt(DiamondsCollectedKey, value); }
        }
        private float height = 3f;

        [Inject]
        private void Construct(ScoreManager scoreManager, GameController gameController)
        {
            ScoreManager = scoreManager;
            GameController = gameController;
        }

        private void Start()
        {
            collectibles = new List<Collectible>();
            GameController.OnRetry += ClearCollectibles;
        }

        public void TrySetupCollectible(Block block)
        {
            if (CanSpawnCollectable())
                SpawnCollectable(block);
        }

        private bool CanSpawnCollectable() => UnityEngine.Random.Range(0, 2) > 0;

        private void SpawnCollectable(Block block)
        {
            Vector3 position =
                block.Direction == Direction.Right
                    ? new Vector3(UnityEngine.Random.Range(0, block.transform.localScale.x / 2f), height, 0)
                    : new Vector3(0, height, UnityEngine.Random.Range(0, block.transform.localScale.x / 2f));
            var collectible = Instantiate(
                _collectiblePrefab,
                block.transform.position + position,
                Quaternion.identity
            );

            collectible.OnCollect += ScoreManager.UpdateScore;
            collectible.OnCollect += UpdateScore;

            collectible.OnDestroy += RemoveDiamondFromList;
            collectibles.Add(collectible);
        }

        private void RemoveDiamondFromList(Collectible diamond)
        {
            collectibles.Remove(diamond);
        }

        private void ClearCollectibles()
        {
            collectibles.ForEach(x => Destroy(x.gameObject));
            collectibles.Clear();
        }

        private void UpdateScore(int value)
        {   
            var total = Collected +1;
            Collected = total;
            OnUpdateCollectiblesScore?.Invoke();
        }

        private void OnDestroy()
        {
            GameController.OnRetry -= ClearCollectibles;
        }
    }
}
