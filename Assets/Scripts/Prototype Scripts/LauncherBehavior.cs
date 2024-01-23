using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherBehavior : MonoBehaviour
{

    public float launchForce = 10f;
    public float speedIncrease = 3f;
    public float additionalGravity = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody hb = other.GetComponent<Rigidbody>();

            if (hb != null)
            {
                Vector3 launchDirection = transform.up;
                hb.AddForce(launchDirection * launchForce, ForceMode.Impulse);

                hb.velocity += hb.velocity.normalized * speedIncrease;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody hb = other.GetComponent<Rigidbody>();

            if (hb != null)
            {
                Vector3 additionalGravityForce = Vector3.down * additionalGravity * Time.deltaTime;
                hb.AddForce(additionalGravityForce, ForceMode.Acceleration);
            }

        }
    }

}
