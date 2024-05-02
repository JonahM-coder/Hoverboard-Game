using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraBehaviorSolo : MonoBehaviour
{

    public GameObject player;
    public GameObject cameraTarget;
    
    public float speed;
    public string playerTag;

    public float zoomSpeed = 15f;
    public float rotationSpeed = 15f;

    private bool isActive = false;
    private bool isZoomedOut = false;
    private bool isRotating = false;

    // Start is called before the first frame update
    private void Awake()
    {
        StartCoroutine(Countdown());
        FindPlayer();

        // Get camera to face player collection object on first frame
        gameObject.transform.position = Vector3.Lerp(transform.position, cameraTarget.transform.position, Time.deltaTime * speed);
        gameObject.transform.LookAt(player.gameObject.transform.position);

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 cameraInput = InputSystem.GetDevice<Gamepad>().rightStick.ReadValue();

        if (isActive)
        {
            // Follow player collection object
            Follow();

            // Zoom camera controls
            if (Mathf.Abs(cameraInput.y) > 0.1f)
            {
                if (cameraInput.y > 0)
                {
                    ZoomCamera(1);
                }
                else
                {
                    ZoomCamera(-1);
                }
            }

        }
        
    }

    private void Follow()
    {
        gameObject.transform.position = Vector3.Lerp(transform.position, cameraTarget.transform.position, Time.deltaTime * speed);
        gameObject.transform.LookAt(player.gameObject.transform.position);
    }

    private void FindPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);
        if (players.Length > 0)
        {
            player = players[0];
            cameraTarget = player.transform.Find("CameraTarget")?.gameObject;
        }
        else
        {
            Debug.LogError("No GameObject with tag '" + playerTag + "' found.");
        }
    }

    private void ZoomCamera(float input)
    {
        Vector3 offset = transform.position - cameraTarget.transform.position;
        offset += transform.forward * input * zoomSpeed * Time.deltaTime;
        transform.position = cameraTarget.transform.position + offset;
    }

    private void RotateCamera(float input)
    {
        transform.RotateAround(cameraTarget.transform.position, Vector3.up, input * rotationSpeed * Time.deltaTime);
    }

    private void ToggleZoom()
    {
        isZoomedOut = !isZoomedOut;
    }

    IEnumerator Countdown()
    {
        int count = 3;

        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
        }

        isActive = true;
    }

}