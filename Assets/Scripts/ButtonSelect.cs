using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ButtonSelect : MonoBehaviour
{

    public EventSystem boardSelectSystem;

    public GameObject levelSelectMenu;
    public GameObject boardSelectMenu;

    public GameObject levelSelectButton;
    public GameObject boardSelectButton;

    private bool isSelecting;
    private bool isReturning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 moveInput = InputSystem.GetDevice<Gamepad>().leftStick.ReadValue();
        isSelecting = InputSystem.GetDevice<Gamepad>().aButton.isPressed;
        isReturning = InputSystem.GetDevice<Gamepad>().bButton.isPressed;

        if (!boardSelectMenu.activeInHierarchy && isSelecting)
        {
            EventSystem.current.SetSelectedGameObject(null);

            if(levelSelectMenu)
            {
                EventSystem.current.SetSelectedGameObject(boardSelectButton);
            }

        }

        if (!levelSelectMenu.activeInHierarchy && isSelecting)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(levelSelectButton);
        }

    }
}
