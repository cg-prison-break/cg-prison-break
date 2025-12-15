using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        var rnd = new System.Random();
        spawnedNPCs.Clear();

        for (int i = 0; i < maxNPCs; i++)
        {
            var spawnPoints = spawnPointContainer.GetComponentsInChildren<SpawnPoint>();
            var spawnPoint = spawnPoints[rnd.Next(spawnPoints.Length)];

            // spawn on navmesh
            if (!NavMeshUtils.TryFindValidNavMeshPosition(spawnPoint.transform.position, spawnRadius, out var validPos))
            {
                // fallback to spawn point position
                validPos = spawnPoint.transform.position;
            }

            GameObject npc = Instantiate(npcPrefab, validPos, Quaternion.identity);

            var agent = npc.GetComponent<NavMeshAgent>();
            agent.Warp(npc.transform.position);

            if (npc.TryGetComponent<NPC>(out var npcComponent))
            {
                agent.speed = npcComponent.speed;
            }
            else
            {
                // fallback to default speed
                agent.speed = 2.0f;
            }

            spawnedNPCs.Add(npc);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
