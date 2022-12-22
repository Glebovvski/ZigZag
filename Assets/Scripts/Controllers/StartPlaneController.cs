using UnityEngine;

public class StartPlaneController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private const string FallAnim = "Fall";
    
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            _animator.Play(FallAnim);
        }
    }
}
