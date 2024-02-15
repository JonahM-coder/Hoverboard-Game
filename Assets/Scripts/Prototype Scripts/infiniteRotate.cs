using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infiniteRotate : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // The axis around which the object will rotate
    public float rotationSpeed = 30f; // Rotation speed in degrees per second

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around the specified axis
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
