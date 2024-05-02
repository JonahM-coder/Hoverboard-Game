using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseBehavior : MonoBehaviour
{

    //Pause Menu Components
    public GameObject pauseMenu, quitMenu;

    //Transition Components
    public GameObject pauseFirstButton, resetButton, quitButton, exitFirstButton, exitClosedButton;

    //State variables
    private bool isPausable = false;

    public void Start()
    {
        //Prevent pausing during countdown
        Time.timeScale = 1f;
        StartCoroutine(Countdown());

        //Disable Pause UI menu during countdown
        quitMenu.SetActive(false);
        pauseMenu.SetActive(false);

    }

    public void OnPausePerformed()
    {
        if (isPausable)
        {
            if (!pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;

                //clear selected object and set new selected object
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(pauseFirstButton);
                resetButton.SetActive(true);
                quitButton.SetActive(true);
                
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void OpenQuit()
    {
        quitMenu.SetActive(true);

        //clear selected object and set new selected object
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(exitFirstButton);
    }

    public void CloseQuit()
    {
        quitMenu.SetActive(false);

        //clear selected object and set new selected object
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(quitButton);
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
