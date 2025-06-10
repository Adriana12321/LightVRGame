using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    public GameObject chunkPrefab;
    public Transform player;
    public Transform firstChunkInScene; // drag the first chunk here
    public int chunksAhead = 3;
    public int maxChunks = 20;
    public float spawnDistance = 50f;

    private Vector3 nextSpawnPosition = Vector3.zero;
    private bool initialized = false;
    private int currentChunkCount = 0;

    void Start()
    {
        if (firstChunkInScene == null)
        {
            Debug.LogError("[ChunkSpawner] No firstChunkInScene assigned!");
            return;
        }

        Transform endPoint = firstChunkInScene.Find("EndPoint");
        if (endPoint == null)
        {
            Debug.LogError("[ChunkSpawner] First chunk is missing EndPoint!");
            return;
        }

        nextSpawnPosition = endPoint.position;
        initialized = true;

        Debug.Log($"[ChunkSpawner] Initialized with first chunk end at {nextSpawnPosition}");

        for (int i = 0; i < chunksAhead && currentChunkCount < maxChunks; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
        if (!initialized || currentChunkCount >= maxChunks) return;

        if (player.position.x + spawnDistance > nextSpawnPosition.x)
        {
            SpawnNextChunk();
        }
    }

    void SpawnNextChunk()
    {
        if (currentChunkCount >= maxChunks)
        {
            Debug.Log("[ChunkSpawner] Max chunks reached. No more chunks will be spawned.");
            return;
        }

        GameObject chunk = Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity);

        Transform startPoint = chunk.transform.Find("StartPoint");
        Transform endPoint = chunk.transform.Find("EndPoint");

        if (startPoint != null && endPoint != null)
        {
            Vector3 offset = nextSpawnPosition - startPoint.position;
            chunk.transform.position += offset;

            Debug.Log($"[ChunkSpawner] Spawned chunk #{currentChunkCount + 1} at {chunk.transform.position}, aligned to {nextSpawnPosition}");
            nextSpawnPosition = endPoint.position;
        }
        else
        {
            Debug.LogWarning("[ChunkSpawner] Missing StartPoint or EndPoint on chunk prefab, using fallback.");
            chunk.transform.position = nextSpawnPosition;
            nextSpawnPosition += new Vector3(spawnDistance, 0, 0);
        }

        currentChunkCount++;
    }
}
