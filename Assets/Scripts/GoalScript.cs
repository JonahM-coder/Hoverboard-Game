using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GoalScript : MonoBehaviour
{
    //Camera Change
    public Transform stopPoint;
    public Camera mainCamera;
    public Camera goalCamera;

    //Text Elements
    public Text goalText;
    public Text timeLeftText;
    public Text currentTimeText;
    public Text currentGateText;
    public Text powerText;
    public Text currentPowerText;
    public Text speedometerText;
    public Text finalTimeText;
    public BoostmeterBar boostBar;

    //Goal UI GameObject
    public GameObject landingMenu, quitMenu;
    public GameObject landingButton, quitButton, quitMenu_landingButton, quitMenu_exitGameButton;
    
    private bool goalReached = false;

    public void Start()
    {
        //Disable Menus
        landingMenu.SetActive(false);
        quitMenu.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        goalText.enabled = false;

        if (other.CompareTag("Player") && !goalReached)
        {
            goalReached = true;

            mainCamera.enabled = false;
            goalCamera.enabled = true;

            HbController controller = other.GetComponent<HbController>();
            if (controller != null)
            {
                controller.StopAtPoint(stopPoint.position);
            }

            //Disable Text
            DisableHUD();

            // Victory screen text goes here
            goalText.enabled = true;
            finalTimeText.enabled = true;
            landingMenu.SetActive(true);

            if (!landingMenu.activeInHierarchy)
            {
                SelectButtonAndEnableNavigation(landingButton);
            }

        }
    }

    public void OpenQuit()
    {
        quitMenu.SetActive(true);
        SelectButtonAndEnableNavigation(landingButton);
    }

    public void CloseQuit()
    {
        quitMenu.SetActive(false);
        SelectButtonAndEnableNavigation(quitButton);
    }

    private void SelectButtonAndEnableNavigation(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    private void DisableHUD()
    {
        // In-game Text is disabled
        timeLeftText.enabled = false;
        currentTimeText.enabled = false;
        currentGateText.enabled = false;
        powerText.enabled = false;
        currentPowerText.enabled = false;
        speedometerText.enabled = false;
        boostBar.gameObject.SetActive(false);
    }

}
