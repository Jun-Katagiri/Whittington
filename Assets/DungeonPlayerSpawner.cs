using UnityEngine;
using DunGen;
using System.Collections; // Required for Coroutines

public class DungeonPlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    private RuntimeDungeon dungeon;

    void Awake()
    {
        dungeon = GetComponent<RuntimeDungeon>();
        dungeon.Generator.OnGenerationStatusChanged += OnStatusChanged;
    }

    void OnStatusChanged(DungeonGenerator generator, GenerationStatus status)
    {
        if (status == GenerationStatus.Complete)
        {
            StartCoroutine(SpawnSequence());
        }
    }

    IEnumerator SpawnSequence()
    {
        // 1. Wait for End of Frame to ensure DunGen geometry is physically in the world
        yield return new WaitForEndOfFrame();
        Physics.SyncTransforms();

        // 2. Determine Spawn Position (Force 0,1,0 if you want simple testing)
        // If you want to use the marker, uncomment the next line:
        // var startTile = dungeon.Generator.CurrentDungeon.MainPathTiles[0];
        Vector3 spawnPos = new Vector3(0f, 1.5f, 0f); // 1.5m up to clear the floor

        // 3. Instantiate DISABLED to prevent "pop"
        GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        
        // 4. Find the Brain (CharacterController)
        var controller = player.GetComponentInChildren<CharacterController>();
        
        if (controller != null)
        {
            controller.enabled = false; // Turn off physics
            
            // 5. Force Position HARD
            player.transform.position = spawnPos;
            
            // 6. Wait one more physics frame to let the transform settle
            yield return new WaitForFixedUpdate();
            
            controller.enabled = true; // Turn brain back on
            Debug.Log($"[SPAWN SUCCESS] Player locked at {player.transform.position}");
        }
    }
}