using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LandingPageBehavior : MonoBehaviour
{
    //Gamepad component
    private Gamepad gamepad;

    //Menu Components
    public GameObject landingMenu, instructionMenu;

    //Button Components
    public GameObject startButton, manualButton, quitButton;

    public void Start()
    {
        landingMenu.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(startButton);
        
        instructionMenu.SetActive(false);
    }

    private void OnEnable()
    {
        // Enable the controls when the script is enabled
        gamepad = Gamepad.current;
    }

    private void OnDisable()
    {
        // Disable the controls when the script is disabled
        gamepad = null;
    }

    void Update()
    {
        if (gamepad != null)
        {
            var move = gamepad.leftStick.ReadValue();
            if (move.magnitude > 0.5f)
            {
                var current = EventSystem.current.currentSelectedGameObject?.GetComponent<Selectable>();
                if (current != null)
                {
                    var next = current.FindSelectable(move);
                    if (next != null)
                    {
                        EventSystem.current.SetSelectedGameObject(next.gameObject);
                    }
                }
            }
        }

    }

    public void OpenInstructionMenu()
    {
        instructionMenu.SetActive(true);
        landingMenu.SetActive(false);
    }

    public void CloseInstructionMenu()
    {
        instructionMenu.SetActive(false);
        landingMenu.SetActive(true);

        // Clear selected object and set new selected object
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(manualButton);
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }


}
