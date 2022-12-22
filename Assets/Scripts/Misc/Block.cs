using System;
using Managers;
using UnityEngine;
using UnityEngine.AI;

public class Block : MonoBehaviour
{
    private const float width = 3f;
    private const string FallAnim = "Fall";

    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private Animator animator;

    private Direction direction;
    public Direction Direction => direction;
    private Vector3 rotation;

    private Vector3 right = Vector3.zero;
    private Vector3 left = new Vector3(0, -90, 0);

    public bool IsCurrent { get; private set; }
    public bool Fell { get; private set; } = false;

    public event Action<Block> OnFall;

    public Vector3 Position
    {
        get { return transform.position; }
        private set { transform.position = value; }
    }
    public Vector3 Size
    {
        get { return transform.localScale; }
        private set { transform.localScale = value; }
    }

    public Vector3 EndPosition
    {
        get
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            return direction == Direction.Right
                ? transform.position + new Vector3(Size.x, 0, 0)
                : transform.position + new Vector3(0, 0, Size.x);
        }
    }

    public void SetSize(float size) => Size = new Vector3(size, Size.y, width);

    public void SetDirection(Direction direction)
    {
        this.direction = direction;
        rotation = direction == Direction.Right ? right : left;
        transform.rotation = Quaternion.Euler(rotation);
    }

    public void SetPosition(Vector3 position, bool isStartPos = false)
    {
        if (isStartPos)
        {
            if (direction == Direction.Left)
                Position = position;
            else
                Position = position + new Vector3(0, 0, width / 2f);
            return;
        }

        if (direction == Direction.Right)
        {
            Position = position + new Vector3(-width / 2f, 0, 0);
        }
        else
        {
            Position = position + new Vector3(Size.z / 2f, 0, -width / 2f);
        }
    }

    public void Fall()
    {
        if (Fell) return;
        
        animator.Play(FallAnim);
        Fell = true;
        OnFall?.Invoke(this);
    }

    public void ResetAnim() => Fell = false;

    public void ResetCurrent() => IsCurrent = false;

    public void BuildNavMesh() => navMeshSurface.BuildNavMesh();

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            IsCurrent = false;
            Fall();
        }
    }
}
