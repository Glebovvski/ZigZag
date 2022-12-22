using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    private PlayerInput Ball{get;set;}

    private Vector3 distanceToPlayer;

    [Inject]
    private void Construct(PlayerInput playerInput)
    {
        Ball = playerInput;
    }

    private void Start()
    {
        distanceToPlayer = this.transform.position - Ball.transform.position;
    }

    private void Update()
    {
        this.transform.position = new Vector3(Ball.transform.position.x + distanceToPlayer.x, this.transform.position.y, Ball.transform.position.z + distanceToPlayer.z);
    }
}
