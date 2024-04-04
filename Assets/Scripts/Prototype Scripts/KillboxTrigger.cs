using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillboxTrigger : MonoBehaviour
{

    public GameObject retireSprite;

    public void Start()
    {
        retireSprite.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollection"))
        {

            Debug.Log("Killbox collided!");

        }
    }

}
