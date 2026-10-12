using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Chunk[] chunkPrefabs;
    [SerializeField] private float spawnAheadDistance = 120f;
    [SerializeField] private float despawnBehindDistance = 30f;
    [SerializeField] private int safeChunksAtStart = 2;
    [SerializeField] private float distancePerTier = 400f;

    private readonly Dictionary<Chunk, ObjectPool<Chunk>> pools = new();
    private readonly Queue<Chunk> activeChunks = new();
    private readonly List<Chunk> eligible = new();
    private float nextSpawnZ;
    private int spawnedCount;

    private void Awake()
    {
        foreach (Chunk prefab in chunkPrefabs)
        {
            Chunk p = prefab;
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

    private void Start() => nextSpawnZ = player.position.z - 10f;

    private void Update()
    {
        // The guard stops a bad setup from freezing the editor.
        int guard = 0;
        while (nextSpawnZ < player.position.z + spawnAheadDistance && guard++ < 50)
            SpawnChunk();

        while (activeChunks.Count > 0 &&
               activeChunks.Peek().transform.position.z + activeChunks.Peek().Length
                   < player.position.z - despawnBehindDistance)
        {
            Chunk old = activeChunks.Dequeue();
            pools[old.Prefab].Release(old);
        }
    }

    private Chunk PickChunk()
    {
        if (spawnedCount < safeChunksAtStart)
            return chunkPrefabs[0]; // must be a tier 0 chunk

        int maxTier = 1 + Mathf.FloorToInt(nextSpawnZ / distancePerTier);

        eligible.Clear();
        foreach (Chunk c in chunkPrefabs)
            if (c.Tier <= maxTier) eligible.Add(c);

        if (eligible.Count == 0) return chunkPrefabs[0];
        return eligible[Random.Range(0, eligible.Count)];
    }

    private void SpawnChunk()
    {
        Chunk prefab = PickChunk();
        spawnedCount++;

        Chunk chunk = pools[prefab].Get();
        chunk.transform.position = new Vector3(0f, 0f, nextSpawnZ);
        activeChunks.Enqueue(chunk);

        if (chunk.Length <= 0f)
            Debug.LogError($"{prefab.name} has Length {chunk.Length}. Set it to 30 on the prefab root.");

        nextSpawnZ += Mathf.Max(chunk.Length, 1f);
    }
}