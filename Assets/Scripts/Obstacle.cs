using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Obstacle touched by {other.name}");
        if (other.CompareTag("Player"))
            GameManager.Instance.GameOver();
    }
}

