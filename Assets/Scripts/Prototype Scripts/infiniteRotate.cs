using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infiniteRotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // Rotation speed in degrees per second

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around the specified axis
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}