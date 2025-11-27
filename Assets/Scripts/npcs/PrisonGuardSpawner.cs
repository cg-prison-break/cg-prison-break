using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PrisonGuardSpawner : MonoBehaviour
{
    [Range(1, 20)]
    public int maxGuards = 5;
    public float spawnRadius = 3f;
    public GameObject prisonGuardPrefab;
    public List<GameObject> spawnPoints = new();

    private readonly List<GameObject> spawnedGuards = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var rnd = new System.Random();
        spawnedGuards.Clear();

        for (int i = 0; i < maxGuards; i++)
        {
            var spawnPoint = spawnPoints.OrderBy(x => rnd.Next()).Take(1).First();

            // spawn on navmesh
            Vector3 randomDir = UnityEngine.Random.insideUnitSphere * spawnRadius + spawnPoint.transform.position;
            NavMesh.SamplePosition(randomDir, out var navMeshHit, spawnRadius, NavMesh.AllAreas);

            // ensure spawning on the ground
            Physics.Raycast(navMeshHit.position + Vector3.up * 10f, Vector3.down, out var hit, 100f);

            GameObject guard = Instantiate(prisonGuardPrefab, hit.point, Quaternion.identity);

            var agent = guard.GetComponent<NavMeshAgent>();
            agent.speed = 2.0f;
            guard.GetComponent<PrisonGuard>().navMeshAgent = agent;

            spawnedGuards.Add(guard);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
