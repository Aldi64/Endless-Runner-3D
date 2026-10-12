using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Chunk[] chunkPrefabs;
    [SerializeField] private float spawnAheadDistance = 120f;
    [SerializeField] private float despawnBehindDistance = 30f;

    private readonly Dictionary<Chunk, ObjectPool<Chunk>> pools = new();
    private readonly Queue<Chunk> activeChunks = new();
    private float nextSpawnZ;

    private void Awake()
    {
        foreach (Chunk prefab in chunkPrefabs)
        {
            Chunk p = prefab; // capture for the lambdas below
            pools[p] = new ObjectPool<Chunk>(
                createFunc: () =>
                {
                    Chunk c = Instantiate(p, transform);
                    c.Prefab = p;
                    return c;
                },
                actionOnGet: c => c.gameObject.SetActive(true),
                actionOnRelease: c => c.gameObject.SetActive(false),
                actionOnDestroy: c => Destroy(c.gameObject));
        }
    }

    private void Start()
    {
        nextSpawnZ = player.position.z - 10f; // first chunk starts just behind the player
    }

    private void Update()
    {
        while (nextSpawnZ < player.position.z + spawnAheadDistance)
            SpawnChunk();

        while (activeChunks.Count > 0 &&
               activeChunks.Peek().transform.position.z + activeChunks.Peek().Length
                   < player.position.z - despawnBehindDistance)
        {
            Chunk old = activeChunks.Dequeue();
            pools[old.Prefab].Release(old);
        }
    }

    private void SpawnChunk()
    {
        Chunk prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        Chunk chunk = pools[prefab].Get();
        chunk.transform.position = new Vector3(0f, 0f, nextSpawnZ);
        activeChunks.Enqueue(chunk);
        nextSpawnZ += chunk.Length;
    }
}