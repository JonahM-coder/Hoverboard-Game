using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehavior : MonoBehaviour
{
    private Rigidbody hb;
    public float antiGravityForce = 9.81f;

    void Start()
    {
        hb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        hb.AddForceAtPosition(antiGravityForce * hb.mass * Vector3.up, hb.centerOfMass);
    }
}
