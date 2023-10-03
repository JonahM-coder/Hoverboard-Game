using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseButtonSelect : MonoBehaviour
{

    public EventSystem boardSelectSystem;
    public GameObject pauseMenu;

    public GameObject resumeButton;
    public GameObject resetButton;
    public GameObject quitButton;

    private bool isSelecting;
    private bool isReturning;

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = InputSystem.GetDevice<Gamepad>().leftStick.ReadValue();
        isSelecting = InputSystem.GetDevice<Gamepad>().aButton.isPressed;
        isReturning = InputSystem.GetDevice<Gamepad>().bButton.isPressed;

        if (pauseMenu && isSelecting)
        {            
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }

    }
}
