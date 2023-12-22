using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float totalTime = 60f;
    private float currentTime;

    public Text timerText;

    private bool isRunning = false;

    public HbController controller;

    public bool countdownFinished = false;

    public GameObject retireSprite;
    public GameObject goalSprite;

    // Start is called before the first frame update
    private void Start()
    {
        isRunning = true;
        currentTime = 0f;
        UpdateTimerDisplay();
        StartCoroutine(Countdown());

        retireSprite.SetActive(false);
        goalSprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(countdownFinished)
        {
            if (isRunning && controller != null)
            {
                currentTime += Time.deltaTime;

                // Check if the hoverboard has reached the goal
                if (controller.hasReachedGoal)
                {
                    isRunning = false;
                }

                UpdateTimerDisplay();
            }

        }
        
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator Countdown()
    {
        int count = 3;

        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
        }

        countdownFinished = true;
    }

}
