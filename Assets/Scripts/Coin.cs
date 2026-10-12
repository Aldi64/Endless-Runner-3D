using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 180f;

    private void Update() => transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.AddCoin();
        gameObject.SetActive(false);
    }
}