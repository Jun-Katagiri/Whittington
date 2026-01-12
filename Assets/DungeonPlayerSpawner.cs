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
        var startTile = dungeon.Generator.CurrentDungeon.MainPathTiles[0];
        var spawnPoint = startTile.GetComponentInChildren<PlayerSpawnPoint>();

        if (spawnPoint != null)
        {
            // 1. Force Unity to calculate the NEW world positions of the moved tiles
            Physics.SyncTransforms();

            Vector3 finalPos = spawnPoint.transform.position;
            Quaternion finalRot = spawnPoint.transform.rotation;

            GameObject player = Instantiate(playerPrefab, finalPos, finalRot);

            // 2. IMPORTANT: The StarterAssets FPS Controller (CharacterController) 
            // will override the transform unless you momentarily disable it.
            var controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                player.transform.position = finalPos;
                player.transform.rotation = finalRot;
                controller.enabled = true;
            }
        }
        else
        {
            Debug.LogError("Player spawn point not found on start tile.");
        }
    }
}