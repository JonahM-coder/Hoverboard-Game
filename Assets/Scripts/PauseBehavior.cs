using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseBehavior : MonoBehaviour
{

    public GameObject pauseSprite;
    public Image blackOverlay;

    private bool isPaused = false;
    private bool isPausable = false;

    public void Start()
    {
        //Prevent pausing during countdown
        StartCoroutine(Countdown());
        
        //Disable Pause UI menu
        pauseSprite.SetActive(false);
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
        Time.timeScale = 0f;
        isPaused = true;
        SetBlackOverlayAlpha(0.5f);
    }

    private void ResumeGame()
    {
        pauseSprite.SetActive(false);
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
