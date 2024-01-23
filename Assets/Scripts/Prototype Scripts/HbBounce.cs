using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HbBounce : MonoBehaviour
{

    public float bounceForce = 10f;
    public MeshCollider bounceCollider;
    
    private void OnTriggerEnter(Collider other)
    {

        bounceCollider = GetComponent<MeshCollider>();
        
        Vector3 collisionDirection = other.transform.position - transform.position;
        collisionDirection.Normalize();

        Rigidbody hb = GetComponent<Rigidbody>();
        hb.AddForce(collisionDirection * bounceForce, ForceMode.Impulse);
    }

}
