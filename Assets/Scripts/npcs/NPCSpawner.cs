using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum NPCSpawnStates
{
    Idle,
    RandomMovement,
}

public class NPCSpawner : MonoBehaviour
{
    [Range(1, 40)]
    [SerializeField] private int maxNPCs = 5;
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private GameObject spawnPointContainer;
    [SerializeField] private NPCSpawnStates spawnState = NPCSpawnStates.RandomMovement;

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

            GameObject npcGo = Instantiate(npcPrefab, position, Quaternion.identity);
            NPC npc = npcGo.GetComponent<NPC>();
            if (npc == null)
            {
                spawnedNPCs.Add(npcGo);
            }
            else
            {
                switch (spawnState)
                {
                    case NPCSpawnStates.Idle:
                        npc.SpawnState = new IdleState();
                        break;
                    case NPCSpawnStates.RandomMovement:
                        npc.SpawnState = new RandomMovementState();
                        break;
                    default:
                        npc.SpawnState = new RandomMovementState();
                        break;
                }
                spawnedNPCs.Add(npcGo);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
