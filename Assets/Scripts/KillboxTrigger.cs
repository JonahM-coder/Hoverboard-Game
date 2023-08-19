﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillboxTrigger : MonoBehaviour
{

    public Text retireText;
    public Text currentTime;
    public Text timeText; //countdown timer
    public Text currentGateText; //total gates text
    public Text powerText; //Energy text
    public Text currentPowerText; //Current energy text
    public Text speedometerText; //Speedometer
    public Text timeDisplayText; //Time Left text
    public Text timeLeftText;

    public Button restartButton; //Restart button
    public Button menuButton; //Return to Main Menu button

    public HbController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Freeze player in place
            player.DeactivateForce();

            // Turn on retire screen
            retireText.enabled = true;
            currentTime.enabled = false;
            timeText.enabled = false;
            currentGateText.enabled = false;
            powerText.enabled = false;
            currentPowerText.enabled = false;
            speedometerText.enabled = false;
            timeDisplayText.enabled = false;
            timeLeftText.enabled = false;

            restartButton.gameObject.SetActive(true);
            menuButton.gameObject.SetActive(true);

        }
    }

}
