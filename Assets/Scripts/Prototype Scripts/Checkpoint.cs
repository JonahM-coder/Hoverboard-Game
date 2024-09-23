using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber;
    private Vector3 respawnPosition;

    public bool isActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isActive)
            {
                SetRespawnPosition();
            }
        }
    }

    public void SetRespawnPosition()
    {
        respawnPosition = transform.position;
    }

    public void RespawnPlayer(Transform playerTransform)
    {
        playerTransform.position = respawnPosition;
    }
}