using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rider : MonoBehaviour
{
    public LayerMask groundLayer;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision occurred with the ground layer
        if (groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
        {
            Debug.Log("Collided with ground!");
        }
    }

}
