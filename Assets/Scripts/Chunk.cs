using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private float length = 30f;

    public float Length => length;

    // Set by the spawner so the chunk knows which pool it returns to.
    public Chunk Prefab { get; set; }
}