using UnityEngine;
using DunGen;

public class DungeonPlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    private RuntimeDungeon dungeon;

    void Awake()
    {
        dungeon = GetComponent<RuntimeDungeon>();
        // Subscribe to the generation complete event
        dungeon.Generator.OnGenerationStatusChanged += OnStatusChanged;
    }

    void OnStatusChanged(DungeonGenerator generator, GenerationStatus status)
    {
        if (status == GenerationStatus.Complete)
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        // Find the specific 'Start' tile DunGen just placed
        var startTile = dungeon.Generator.CurrentDungeon.MainPathTiles[0];

        // Find our SpawnPoint marker inside that tile using the component
        // This replaces explicit string search: transform.Find("PlayerSpawnPoint")
        var spawnPoint = startTile.GetComponentInChildren<PlayerSpawnPoint>();

        if (spawnPoint != null)
        {
            Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
        else
        {
            // Fallback if you forgot to add the marker
            Instantiate(playerPrefab, startTile.transform.position + Vector3.up, Quaternion.identity);
        }
    }
}