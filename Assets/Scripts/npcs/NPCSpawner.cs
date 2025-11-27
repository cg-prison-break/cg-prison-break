using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    [Range(1, 20)]
    public int maxNPCs = 5;
    public float spawnRadius = 3f;
    public GameObject npcPrefab;
    public List<GameObject> spawnPoints;

    private readonly List<GameObject> spawnedNPCs = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var rnd = new System.Random();
        spawnedNPCs.Clear();

        for (int i = 0; i < maxNPCs; i++)
        {
            var spawnPoint = spawnPoints[rnd.Next(spawnPoints.Count)];

            // spawn on navmesh
            Vector3 randomDir = UnityEngine.Random.insideUnitSphere * spawnRadius + spawnPoint.transform.position;
            NavMesh.SamplePosition(randomDir, out var navMeshHit, spawnRadius, NavMesh.AllAreas);

            // ensure spawning on the ground
            Physics.Raycast(navMeshHit.position + Vector3.up * 10f, Vector3.down, out var hit, 100f);

            GameObject npc = Instantiate(npcPrefab, hit.point, Quaternion.identity);

            var agent = npc.GetComponent<NavMeshAgent>();
            agent.speed = 2.0f;

            spawnedNPCs.Add(npc);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
