using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateTrigger : MonoBehaviour
{
    private int totalGates = 0;
    public Text gateText;

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Gate" || other.transform.tag == "Checkpoint")
        {

            //Add gate to total score
            totalGates++;

            //Upload score to gate text
            gateText.text = "Gates: " + totalGates.ToString();

            // Destroy the time pickup object
            Destroy(other.gameObject);
        }
    }
}
