using UnityEngine;
using Zenject;

public class ExitTrigger : MonoBehaviour
{
    private GameController GameController{get;set;}

    [SerializeField] private Transform ball;

    [Inject]
    private void Construct(GameController gameController)
    {
        GameController = gameController;
    }

    private void Update()
    {
        this.transform.position = new Vector3(
            ball.position.x,
            this.transform.position.y,
            ball.position.z
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameController.Lose();
        }
    }
}
