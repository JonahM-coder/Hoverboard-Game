using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBehavior : MonoBehaviour
{
    public GameObject speedometer;
    public GameObject boostBar;
    public GameObject timeLeft;
    public GameObject timeLeftText;
    public GameObject background;

    public bool isActive;

    private void Start()
    {
        speedometer.SetActive(false);
        boostBar.SetActive(false);
        timeLeft.SetActive(false);
        timeLeftText.SetActive(false);
        background.SetActive(false);

        StartCoroutine(Countdown());
    }

    private void Update()
    {
        if (isActive)
        {
            speedometer.SetActive(true);
            boostBar.SetActive(true);
            timeLeft.SetActive(true);
            timeLeftText.SetActive(true);
            background.SetActive(true);
        }
    }

    IEnumerator Countdown()
    {
        int count = 3;

        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
        }

        isActive = true;

    }

}
