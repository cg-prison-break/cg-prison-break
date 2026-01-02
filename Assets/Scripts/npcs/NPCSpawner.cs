using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Range(1, 40)]
    public int maxNPCs = 5;
    public float spawnRadius = 3f;
    public GameObject npcPrefab;
    public GameObject spawnPointContainer;

    private readonly List<GameObject> spawnedNPCs = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnNPCs());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SpawnNPCs()
    {
        var rnd = new System.Random();
        spawnedNPCs.Clear();

        for (int i = 0; i < maxNPCs; i++)
        {
            var spawnPoints = spawnPointContainer.GetComponentsInChildren<SpawnPoint>();
            var spawnPoint = spawnPoints[rnd.Next(spawnPoints.Length)];
            // spawn on navmesh
            if (!NavMeshUtils.TryFindValidNavMeshPosition(spawnPoint.transform.position, spawnRadius, 0.5f, out var position))
            {
                // fallback to spawn point position
                position = spawnPoint.transform.position;
            }

            GameObject npc = Instantiate(npcPrefab, position, Quaternion.identity);
            spawnedNPCs.Add(npc);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
