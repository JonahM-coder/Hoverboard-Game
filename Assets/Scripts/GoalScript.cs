using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour
{

    public Transform stopPoint;
    public Camera mainCamera;
    public Camera goalCamera;

    //UI Elements
    public Text goalText;
    public Text timeLeftText;
    public Text currentTimeText;
    public Text currentGateText;
    public Text powerText;
    public Text currentPowerText;
    public Text speedometerText;
    public Text finalTimeText;
    public BoostmeterBar boostBar;

    public GameObject goalSprite;
    private bool goalReached = false;

    public void Start()
    {
        //Disable buttons
        goalSprite.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        goalText.enabled = false;

        if (other.CompareTag("Player") && !goalReached)
        {
            goalReached = true;

            mainCamera.enabled = false;
            goalCamera.enabled = true;

            HbController controller = other.GetComponent<HbController>();
            if (controller != null)
            {
                controller.StopAtPoint(stopPoint.position);
            }

            // Victory screen text goes here
            goalText.enabled = true;
            finalTimeText.enabled = true;
            goalSprite.SetActive(true);

            // In-game HUD is disabled
            timeLeftText.enabled = false;
            currentTimeText.enabled = false;
            currentGateText.enabled = false;
            powerText.enabled = false;
            currentPowerText.enabled = false;
            speedometerText.enabled = false;
            boostBar.gameObject.SetActive(false);

        }
    }
}
