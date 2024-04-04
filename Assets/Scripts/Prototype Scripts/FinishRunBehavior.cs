using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FinishRunBehavior : MonoBehaviour
{

    // Pause Menu Components
    public GameObject endMenu, quitMenu;

    // Transition Components
    public GameObject resetButton, quitButton, exitFirstButton, exitClosedButton;

    // State variables
    public bool runComplete;

    // Timer variable
    public Timer timer;

    public void Start()
    {
        // Prevent pausing during countdown
        StartCoroutine(Countdown());

        // Disable Pause UI menu during countdown
        endMenu.SetActive(false);
        quitMenu.SetActive(false);

        runComplete = false;

    }

    public void OnCompletionPerformed()
    {

        if (runComplete)
        {
            if (!endMenu.activeInHierarchy)
            {
                endMenu.SetActive(true);
                

                // Clear selected object and set new selected object
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(resetButton);
                resetButton.SetActive(true);
                quitButton.SetActive(true);
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Goal")
        {
            runComplete = true;
        }
    }

    public void OpenQuit()
    {
        quitMenu.SetActive(true);

        // Clear selected object and set new selected object
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(exitFirstButton);
    }

    public void CloseQuit()
    {
        quitMenu.SetActive(false);

        // Clear selected object and set new selected object
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
    }

}
