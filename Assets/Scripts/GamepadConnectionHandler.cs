using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GamepadConnectionHandler : MonoBehaviour
{

    public GameObject controllerDisconnectedSprite;
    public GameObject background;
    
    void Awake()
    {
        controllerDisconnectedSprite.SetActive(false);
        background.SetActive(false);

        Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    // Update is called once per frame
    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            switch (change)
            {
                // Enable controller disconnection screen
                case InputDeviceChange.Disconnected:
                    Debug.Log("Gamepad disconnected: " + device.displayName);
                    controllerDisconnectedSprite.SetActive(true);
                    background.SetActive(true);
                    Time.timeScale = 0f;
                    break;
                
                // Disable controller disconnection screen
                case InputDeviceChange.Reconnected:
                    Debug.Log("Gamepad reconnected: " + device.displayName);
                    controllerDisconnectedSprite.SetActive(false);
                    background.SetActive(false);
                    Time.timeScale = 1f;
                    break;
            }
        }
    }
}
