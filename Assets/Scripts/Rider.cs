using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rider : MonoBehaviour
{
    public LayerMask groundLayer;
    public NewHbController controller;
    public TimeLeft timeLeftScript;
    public Timer timerScript;
    public FinishRunBehavior finishRunScript;

    public GameObject retireSprite;

    private void Start()
    {
        finishRunScript.runComplete = false;
        retireSprite.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the ground layer
        if (groundLayer == (groundLayer | (1 << other.gameObject.layer)))
        {
            Debug.Log("Triggered with ground!");

            if (controller.currentBoostMeter == 0)
            {
                Debug.Log("Player Eliminated!");
                timerScript.StopTimer();
                timeLeftScript.StopTimer();
                finishRunScript.runComplete = true;
                finishRunScript.OnCompletionPerformed();
                retireSprite.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                controller.Respawn();
            }

        }
    }
}
