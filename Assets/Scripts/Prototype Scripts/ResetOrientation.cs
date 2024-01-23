using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOrientation : MonoBehaviour
{
    public float resetOrientationThreshold = 45f;
    private Quaternion initialRotation;
    private Vector3 initialPosition;
    Rigidbody hb;
    public GameObject rider;

    // Start is called before the first frame update
    private void Start()
    {
        initialRotation = transform.rotation;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (rider.transform.tag == "Ground")
        {
            // Check if the hoverboard is tilted beyond the threshold angle
            if (Quaternion.Angle(transform.rotation, initialRotation) > resetOrientationThreshold)
            {
                ChangeOrientation();
                return;
            }
        }

    }

    private void ChangeOrientation()
    {
        transform.rotation = initialRotation;
        transform.position = initialPosition;

        hb.velocity = Vector3.zero;
        hb.angularVelocity = Vector3.zero;
        transform.rotation = initialRotation;
    }
}
