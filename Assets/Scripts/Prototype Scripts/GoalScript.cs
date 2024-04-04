using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GoalScript : MonoBehaviour
{
    // Camera Change
    public Transform stopPoint;
    public Camera mainCamera;
    public Camera goalCamera;

    // Text Elements
    public GameObject timer;
    public GameObject speedometer;
    public Text finalTimeText;
    public BoostmeterBar boostBar;

    // Goal UI GameObject
    public GameObject landingMenu, quitMenu;
    public GameObject restartButton, quitButton, quitMenu_landingButton, quitMenu_exitGameButton;
    public GameObject goalSprite;

    private bool goalReached = false;

    public GameObject playerCollection; // Reference to the PlayerCollection object

    public void Start()
    {
        // Disable Menus
        landingMenu.SetActive(false);
        quitMenu.SetActive(false);
        goalSprite.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!goalReached)
        {

            // Victory screen text goes here
            goalSprite.SetActive(true);
            finalTimeText.enabled = true;
            landingMenu.SetActive(true);

            // Enable button events
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(restartButton);

            // Swap Cameras
            mainCamera.enabled = false;
            goalCamera.enabled = true;

            // Turn on goal sprite
            goalSprite.SetActive(true);

            // Disable Player HUD
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
        timer.SetActive(false);
        speedometer.SetActive(false);
        boostBar.gameObject.SetActive(false);
    }
}
