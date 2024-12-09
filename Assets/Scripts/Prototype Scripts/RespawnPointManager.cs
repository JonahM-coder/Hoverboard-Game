using System.Collections.Generic;
using UnityEngine;

public class RespawnPointManager : MonoBehaviour
{
    private Dictionary<int, Checkpoint> checkpointsDictionary = new Dictionary<int, Checkpoint>();

    void Start()
    {
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();

        List<Checkpoint> spawnCheckpoints = new List<Checkpoint>();

        // Find checkpoints with the "Spawn" tag and named "Respawn Point"
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.CompareTag("Spawn"))
            {
                spawnCheckpoints.Add(checkpoint);
            }
        }

        spawnCheckpoints.Sort((a, b) => a.gameObject.name.CompareTo(b.gameObject.name));

        for (int i = 0; i < spawnCheckpoints.Count; i++)
        {

            // Disable the MeshRenderer component of the checkpoint
            MeshRenderer meshRenderer = spawnCheckpoints[i].GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            spawnCheckpoints[i].checkpointNumber = i;
            checkpointsDictionary.Add(i, spawnCheckpoints[i]);
        }
    }

    public Checkpoint GetCheckpointByID(int id)
    {
        if (checkpointsDictionary.ContainsKey(id))
        {
            return checkpointsDictionary[id];
        }
        else
        {
            Debug.LogWarning("Checkpoint with ID " + id + " does not exist.");
            return null;
        }
    }
}
