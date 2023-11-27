using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public GameObject levelSelectMenu;
    public GameObject boardSelectMenu;

    public GameObject boardSelectButton; //Landing button on board select menu
    public GameObject levelSelectButton; //Landing button on level select menu

    public GameObject quitLevelSelectButton; //Returning button on level select menu
    public GameObject startLevelSelectButton;

    private Gamepad gamepad;

    void Start()
    {
        boardSelectMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(boardSelectButton);
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

        if (gamepad != null && gamepad.startButton.wasPressedThisFrame)
        {
            Menu();
        }
    }

    public void Menu()
    {
        if (levelSelectMenu.activeSelf)
        {
            // Show the board select menu and set the selected button
            boardSelectMenu.SetActive(true);
            levelSelectMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(boardSelectButton);
        }
        else if (boardSelectMenu.activeSelf)
        {
            // Hide the board select menu and display the level select menu
            levelSelectMenu.SetActive(true);
            boardSelectMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(startLevelSelectButton); // Set the starting button in the level select menu
        }
    }
}
