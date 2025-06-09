using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    public GameObject chunkPrefab;
    public Transform player;
    public float spawnDistance = 50f;
    public int chunksAhead = 3;

    private float lastZ = 0;

    void Start()
    {
        for (int i = 0; i < chunksAhead; i++)
        {
            SpawnChunk(i * spawnDistance);
        }
        lastZ = (chunksAhead - 1) * spawnDistance;
    }

    void Update()
{
    if (player.position.z + spawnDistance > lastZ)
    {
        lastZ += spawnDistance;
        SpawnChunk(lastZ);
    }
}


    void SpawnChunk(float zPos)
    {
        Vector3 pos = new Vector3(0, 0, zPos);
        Instantiate(chunkPrefab, pos, Quaternion.identity);
    }


}
