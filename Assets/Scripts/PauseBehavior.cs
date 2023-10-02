using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehavior : MonoBehaviour
{

    public GameObject pauseSprite;
    private bool isPaused = false;
    private bool isPausable = false;

    public void Start()
    {
        StartCoroutine(Countdown());
        pauseSprite.SetActive(false);
    }

    public void OnPausePerformed()
    {

        if(isPausable)
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

    }

    private void PauseGame()
    {
        pauseSprite.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void ResumeGame()
    {
        pauseSprite.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    IEnumerator Countdown()
    {
        int count = 3;

        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
        }
        yield return new WaitForSeconds(1);
        isPausable = true;
    }

}
