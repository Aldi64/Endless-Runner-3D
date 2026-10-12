using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private float length = 30f;
    [SerializeField] private int tier = 0;
    public int Tier => tier;
    private Coin[] coins;

    public float Length => length;
    public Chunk Prefab { get; set; }

    private void Awake() => coins = GetComponentsInChildren<Coin>(true);

    // Runs each time the pool hands this chunk out again.
    private void OnEnable()
    {
        foreach (Coin coin in coins)
            coin.gameObject.SetActive(true);
    }
}