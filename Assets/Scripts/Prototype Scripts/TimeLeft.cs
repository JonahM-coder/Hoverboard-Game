using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TimeLeft : MonoBehaviour
{

    public float startingTime = 20f;
    public float currentTime = 0f;
    public bool timeIsRunning = true;
    
    public Transform stopPoint;

    // UI Text variables
    public BoostmeterBar boostBar;
    public Text timeText; //countdown timer
    public Text timeDisplayText; //Time Left text
    public GameObject retireMenu; //Retire Menu gameobject
    public GameObject retireSprite; //Retire sprite
    public GameObject restartButton; //Restart button
    public GameObject speedometer;

    public void Start()
    {
        // Turn off player HUD
        retireSprite.SetActive(false);
        boostBar.gameObject.SetActive(false);
        timeText.enabled = false;
        speedometer.SetActive(false);
        timeDisplayText.enabled = false;

        StartCoroutine(Countdown());

        currentTime = startingTime;
        timeIsRunning = false;

        retireMenu.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    public void Update()
    {
        if (timeIsRunning)
        {
            if (currentTime >= 0)
            {
                currentTime -= Time.deltaTime;
                DisplayTime(currentTime);
            }
            else
            {
                timeIsRunning = false;
                DisplayMenu();
            }
        }

    }

    private void DisplayMenu()
    {
        Time.timeScale = 0f;

        timeText.enabled = true;
        speedometer.SetActive(true);
        timeDisplayText.enabled = true;

        retireSprite.SetActive(true);
        retireMenu.SetActive(true);
        restartButton.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restartButton);
        Debug.Log("Time ran out!");
    }

    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

    }

    public void AddTime(float seconds)
    {
        currentTime = currentTime + seconds;
    }

    public void StopTimer()
    {
        timeIsRunning = false;
    }

    IEnumerator Countdown()
    {
        int count = 3;

        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
        }

        timeIsRunning = true;
        boostBar.gameObject.SetActive(true);
        timeText.enabled = true;
        speedometer.SetActive(true);
        timeDisplayText.enabled = true;
    }

}
