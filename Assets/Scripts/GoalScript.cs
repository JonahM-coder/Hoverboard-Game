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
    public GameObject restartButton, quitButton, quitMenu_landingButton, quitMenu_exitGameButton;
    public GameObject goalSprite;
    
    private bool goalReached = false;

    public void Start()
    {
        //Disable Menus
        landingMenu.SetActive(false);
        quitMenu.SetActive(false);
        goalSprite.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        goalText.enabled = false;

        if (other.CompareTag("Player") && !goalReached)
        {
            goalReached = true;

            //Victory screen text goes here
            goalText.enabled = true;
            finalTimeText.enabled = true;
            landingMenu.SetActive(true);

            //Enable button events
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(restartButton);

            //Swap Cameras
            mainCamera.enabled = false;
            goalCamera.enabled = true;

            //Turn on goal sprite
            goalSprite.SetActive(true);

            //Freeze player
            HbController controller = other.GetComponent<HbController>();
            if (controller != null)
            {
                controller.StopAtPoint(stopPoint.position);
            }

            //Disable Player HUD
            DisableHUD();

        }
    }

    public void OpenQuit()
    {
        quitMenu.SetActive(true);
        SelectButtonAndEnableNavigation(restartButton);
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
