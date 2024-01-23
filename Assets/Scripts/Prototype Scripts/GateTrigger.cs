using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateTrigger : MonoBehaviour
{
    //Gate counter
    private int totalGates = 0;
    public Text gateText;

    //Checkpoint Sprite variables
    public GameObject checkpointSprite;
    private bool isCheckpointVisible = false;
    private float checkpointTimer = 0f;
    private float checkpointVisibilityDuration = 2f;

    //Gate Sprite variables
    public GameObject gateSprite;
    private bool isGateVisible = false;
    private float gateTimer = 0f;
    private float gateVisibilityDuration = 1f;

    public void Start()
    {
        checkpointSprite.SetActive(false);
        gateSprite.SetActive(false);
    }

    private void Update()
    {
        //Update Checkpoint time
        if (isCheckpointVisible)
        {
            checkpointTimer += Time.deltaTime;
            if (checkpointTimer >= checkpointVisibilityDuration)
            {
                checkpointTimer = 0f;
                isCheckpointVisible = false;
                checkpointSprite.SetActive(false);
            }
        }

        //Update Gate time
        if (isGateVisible)
        {
            gateTimer += Time.deltaTime;
            if (gateTimer >= gateVisibilityDuration)
            {
                gateTimer = 0f;
                isGateVisible = false;
                gateSprite.SetActive(false);
            }
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Gate")
        {
            isGateVisible = true;
            totalGates++;
            gateText.text = "Gates: " + totalGates.ToString();
            Destroy(other.gameObject);
            gateSprite.SetActive(true);
        }
        
        if (other.transform.tag == "Checkpoint")
        {
            isCheckpointVisible = true;
            totalGates++;
            gateText.text = "Gates: " + totalGates.ToString();
            Destroy(other.gameObject);
            checkpointSprite.SetActive(true);
        }
    }
}
