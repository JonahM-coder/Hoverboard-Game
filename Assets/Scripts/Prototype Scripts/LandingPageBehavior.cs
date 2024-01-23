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
    }

    public void CloseInstructionMenu()
    {
        instructionMenu.SetActive(false);

        //clear selected object and set new selected object
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(manualButton);
    }

}
