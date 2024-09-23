using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    
    public GameObject destroyedVersion;
    public float destroyDelay = 5f;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Instantiate the destroyed version of the object
            GameObject destroyedInstance = Instantiate(destroyedVersion, transform.position, transform.rotation);

            // Destroy the current game object
            Destroy(gameObject);

            // Schedule the destroyed instance to be unloaded after the delay
            Destroy(destroyedInstance, destroyDelay);
        }
    }
}
