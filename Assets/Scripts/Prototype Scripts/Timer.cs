using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float totalTime = 60f;
    private float currentTime;

    public Text timerText;

    public bool isRunning = false;

    public GameObject playerCollection;

    private bool countdownFinished = false;

    public GameObject retireSprite;
    public GameObject goalSprite;

    // Start is called before the first frame update
    private void Start()
    {
        isRunning = false;
        currentTime = 0f;
        UpdateTimerDisplay();
        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (countdownFinished && isRunning)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            StopTimer();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnCollisionEnter(Collision other)
    {
        // Check if collision occurs with the object you want to stop the timer
        if (other.gameObject.CompareTag("Goal"))
        {
            // Stop the timer
            StopTimer();
        }
    }

    public void StopTimer()
    {
        isRunning = false;
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
        isRunning = true;

    }

}
