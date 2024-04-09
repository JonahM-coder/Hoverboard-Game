using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HbGroundCheck : MonoBehaviour
{
    public bool isGrounded = false;
    public bool IsGrounded => isGrounded;

    public float raycastDistance = 1f;

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
                return;
            }
        }

        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
