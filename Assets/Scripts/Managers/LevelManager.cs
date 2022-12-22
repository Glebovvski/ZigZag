using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        private CollectibleManager CollectibleManager { get; set; }
        private PlayerInput PlayerInput { get; set; }

        [SerializeField] private int minSize;

        [SerializeField] private int maxSize;

        [SerializeField] private int poolSize;

        [SerializeField] private Block _blockPrefab;

        [SerializeField] private int blockReorganizeIndex = 0;

        [SerializeField] private int blockFallIndexOffset;

        [SerializeField] private GameObject _startBlock;

        private List<Block> blocks = new List<Block>();
        private List<Block> movingBlocks = new List<Block>();

        public Vector3 NextAIDestiantion { get; private set; }
        private int CurrentBlockIndex { get; set; } = 0;

        [Inject]
        private void Construct(CollectibleManager collectibleManager, PlayerInput playerInput)
        {
            CollectibleManager = collectibleManager;
            PlayerInput = playerInput;
        }

        private void Start()
        {
            InitLevel();
        }

        public void InitLevel()
        {
            ResetBlocks();
            Direction lastDirection = Direction.Right;
            Block block;
            for (int i = 0; i < poolSize; i++)
            {
                if (blocks.Count <= i)
                {
                    block = Instantiate(_blockPrefab);
                    blocks.Add(block);
                }
                else
                    block = blocks[i];

                if (i == 0)
                {
                    var randonDir = Random.Range(0, 2);
                    lastDirection = randonDir > 0 ? Direction.Right : Direction.Left;
                    SetBlockParams(block, lastDirection, true);
                }
                else
                {
                    var direction =
                        lastDirection == Direction.Right ? Direction.Left : Direction.Right;
                    lastDirection = direction;
                    SetBlockParams(block, direction, blocks[i - 1].EndPosition);
                }
                block.OnFall += CurrentBlockChanged;

                CollectibleManager.TrySetupCollectible(block);
            }

            foreach (var surface in blocks)
            {
                surface.BuildNavMesh();
            }

            NextAIDestiantion = blocks[0].EndPosition;
        }

        private void ResetBlocks()
        {
            if (blocks.Count == 0)
                return;

            foreach (var block in blocks)
            {
                block.SetDirection(Direction.Right);
                block.SetPosition(Vector3.zero);
                block.SetSize(1);
            }
        }

        private void SetBlockParams(Block block, Direction direction, bool isFirstElement)
        {
            SetSizeAndDirection(block, direction);
            _startBlock.transform.position = new Vector3(
                _startBlock.transform.position.x,
                0,
                _startBlock.transform.position.z
            );
            block.SetPosition(_startBlock.transform.position, isFirstElement);
        }

        private void SetBlockParams(Block block, Direction direction, Vector3 position)
        {
            SetSizeAndDirection(block, direction);
            position = new Vector3(position.x, 0, position.z);
            block.SetPosition(position);
        }

        private void SetSizeAndDirection(Block block, Direction direction)
        {
            block.SetDirection(direction);
            block.SetSize(Random.Range(minSize, maxSize));
        }

        private void CurrentBlockChanged(Block currentBlock)
        {
            CurrentBlockIndex = blocks.IndexOf(currentBlock);
            NextAIDestiantion = blocks[CurrentBlockIndex + 2].EndPosition;
            var fallIndex = CurrentBlockIndex - blockFallIndexOffset;
            if (fallIndex >= 0)
            {
                blocks[fallIndex].ResetCurrent();
                movingBlocks.Add(blocks[fallIndex]);
                blocks.Remove(blocks[fallIndex]);
            }

            if (movingBlocks.Count < blockReorganizeIndex)
                return;

            MoveBlocks(movingBlocks);
        }

        private void MoveBlocks(List<Block> blocksToMove)
        {
            foreach (var block in blocksToMove)
            {
                block.SetSize(Random.Range(minSize, maxSize));
                block.SetPosition(blocks.Last().EndPosition);
                CollectibleManager.TrySetupCollectible(block);
                blocks.Add(block);
                block.ResetAnim();
            }
            blocksToMove.Clear();
            if (PlayerInput.IsAIControlled)
                blocks.ForEach(x => x.BuildNavMesh());
        }

        internal Vector3 UpdateNextDestination()
        {
            return NextAIDestiantion = blocks[CurrentBlockIndex + 5].EndPosition;
        }
    }

    public enum Direction
    {
        Left = 0,
        Right = 1,
    }
}
