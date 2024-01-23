using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseButtonSelect : MonoBehaviour
{

    public EventSystem pauseMenuSystem;
    
    public GameObject pauseMenu;
    public GameObject resumeButton;
    public GameObject resetButton;
    public GameObject quitButton;

    private bool isSelecting;
    private bool isReturning;
    private bool isPaused;

    // Update is called once per frame
    void Start()
    {
        pauseMenuSystem.SetSelectedGameObject(null);
        pauseMenuSystem.SetSelectedGameObject(resumeButton);
    }
    
    void Update()
    {
        Vector2 moveInput = InputSystem.GetDevice<Gamepad>().leftStick.ReadValue();
        isSelecting = InputSystem.GetDevice<Gamepad>().aButton.isPressed;
        isReturning = InputSystem.GetDevice<Gamepad>().bButton.isPressed;
        isPaused = InputSystem.GetDevice<Gamepad>().startButton.isPressed;

        if (isPaused)
        {
            
            if (pauseMenu.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(resumeButton);
            }
        }

    }
}
