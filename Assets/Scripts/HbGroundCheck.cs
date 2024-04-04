using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HbGroundCheck : MonoBehaviour
{
    public bool isGrounded = false;
    public bool IsGrounded => isGrounded;

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
