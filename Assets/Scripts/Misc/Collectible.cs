using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int Value = 2;
    public event Action<int> OnCollect;
    public event Action<Collectible> OnDestroy;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            OnCollect?.Invoke(Value);
            OnDestroy?.Invoke(this);
            Destroy(this.gameObject);
        }
    }
}
