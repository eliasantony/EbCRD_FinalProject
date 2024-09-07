using System;
using UnityEngine;

public class SecretCoin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddPoints(10000);
            Destroy(gameObject);
        }
    }
}