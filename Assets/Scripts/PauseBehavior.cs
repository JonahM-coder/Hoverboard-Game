using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseBehavior : MonoBehaviour
{

    public GameObject pauseSprite;
    public Image blackOverlay;

    public GameObject resumeButton;
    public GameObject resetButton;
    public GameObject quitButton;

    private bool isPaused = false;
    private bool isPausable = false;

    public void Start()
    {
        //Prevent pausing during countdown
        StartCoroutine(Countdown());
        
        //Disable Pause UI menu
        pauseSprite.SetActive(false);
        resumeButton.SetActive(false);
        resetButton.SetActive(false);
        quitButton.SetActive(false);
        SetBlackOverlayAlpha(0f);
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
        resumeButton.SetActive(true);
        resetButton.SetActive(true);
        quitButton.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        SetBlackOverlayAlpha(0.5f);
    }

    public void ResumeGame()
    {
        pauseSprite.SetActive(false);
        resumeButton.SetActive(false);
        resetButton.SetActive(false);
        quitButton.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SetBlackOverlayAlpha(0f);
    }

    private void SetBlackOverlayAlpha(float alpha)
    {
        Color currentColor = blackOverlay.color;
        currentColor.a = alpha;
        blackOverlay.color = currentColor;
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
