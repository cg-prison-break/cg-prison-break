using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
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
            NavMesh.SamplePosition(randomDir, out NavMeshHit hit, spawnRadius, NavMesh.AllAreas);
            GameObject guard = Instantiate(prisonGuardPrefab, hit.position, Quaternion.identity);

            var agent = guard.GetComponent<BehaviorGraphAgent>();
            agent.Start();

            spawnedGuards.Add(guard);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
