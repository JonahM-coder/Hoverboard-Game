using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiGravityTrack : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.useGravity = false;

                Quaternion trackRotation = transform.rotation;
                rb.rotation = trackRotation;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Quaternion trackRotation = transform.rotation;
                rb.rotation = trackRotation;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.useGravity = true;
            }
        }
    }

}
