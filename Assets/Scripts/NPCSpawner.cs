using UnityEngine;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Prefabs")]
    public GameObject[] npcPrefabs;

    [Header("Defined Spawn Points")]
    public List<Transform> spawnPoints;

    [Header("Spawn Control")]
    public int spawnCountMin = 1;
    public int spawnCountMax = 3;

    void Start()
    {
        if (npcPrefabs == null || npcPrefabs.Length == 0 || spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("[NPCSpawner] Setup incomplete on " + gameObject.name);
            return;
        }

        // Shuffle spawn points to randomize which are chosen
        List<Transform> shuffled = new(spawnPoints);
        Shuffle(shuffled);

        // Clamp and choose random number of spawns
        int spawnCount = Random.Range(spawnCountMin, Mathf.Min(spawnCountMax + 1, shuffled.Count + 1));

        for (int i = 0; i < spawnCount; i++)
        {
            Transform spawnPoint = shuffled[i];
            GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

            GameObject npc = Instantiate(prefab, spawnPoint.position, Quaternion.identity, this.transform);
            Debug.Log($"[NPCSpawner] Spawned {npc.name} at {spawnPoint.name}");
        }
    }

    // Fisher-Yates shuffle
    void Shuffle(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
