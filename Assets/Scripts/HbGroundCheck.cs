using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HbGroundCheck : MonoBehaviour
{
    public bool IsGrounded = false;
    private float raycastDistance = 5f;
    private float maxDistanceToGround = 5f; // Maximum distance to consider the hoverboard as grounded
    private LayerMask groundLayer; // Layer mask for the ground layer

    public void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void FixedUpdate()
    {
        // Perform a raycast downwards to check for the ground layer
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, raycastDistance, groundLayer))
        {
            // Check if the raycast hits the ground layer within maxDistanceToGround
            if (hit.distance <= maxDistanceToGround)
            {
                IsGrounded = true;
                return;
            }
        }

        IsGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entered collider belongs to the ground layer
        if ((groundLayer & (1 << other.gameObject.layer)) != 0)
        {
            IsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exited collider belongs to the ground layer
        if ((groundLayer & (1 << other.gameObject.layer)) != 0)
        {
            IsGrounded = false;
        }
    }
}

