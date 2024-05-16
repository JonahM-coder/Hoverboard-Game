using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBehaviorSolo : MonoBehaviour
{
    public string playerCollectionTag = "PlayerCollection"; // Default tag for PlayerCollection
    public string cameraTag = "CameraTag"; // Tag for CameraTarget

    public float speed = 5f;
    public float zoomSpeed = 15f;
    public float rotationSpeed = 15f;

    private GameObject playerCollection;
    private GameObject cameraTarget;

    public bool isActive = false;

    private void Awake()
    {
        StartCoroutine(Countdown());
        FindPlayerCollectionAndCameraTarget();

        // Initialize camera position and rotation if the cameraTarget is found
        if (cameraTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, cameraTarget.transform.position, Time.deltaTime * speed);
            transform.LookAt(playerCollection.transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (Gamepad.current == null) return;

        Vector2 cameraInput = Gamepad.current.rightStick.ReadValue();

        if (isActive)
        {
            Follow();

            // Zoom camera controls
            if (Mathf.Abs(cameraInput.y) > 0.1f)
            {
                ZoomCamera(cameraInput.y > 0 ? 1 : -1);
            }
        }
    }

    private void Follow()
    {
        if (cameraTarget != null && playerCollection != null)
        {
            transform.position = Vector3.Lerp(transform.position, cameraTarget.transform.position, Time.deltaTime * speed);
            transform.LookAt(playerCollection.transform.position);
        }
    }

    private void FindPlayerCollectionAndCameraTarget()
    {
        playerCollection = GameObject.FindWithTag(playerCollectionTag);
        if (playerCollection != null)
        {
            cameraTarget = FindChildWithTag(playerCollection.transform, cameraTag);
            if (cameraTarget == null)
            {
                Debug.LogError("No child object with tag '" + cameraTag + "' found within PlayerCollection.");
            }
        }
        else
        {
            Debug.LogError("No GameObject with tag '" + playerCollectionTag + "' found.");
        }
    }

    private GameObject FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
            // Recursively search in the child's children
            GameObject result = FindChildWithTag(child, tag);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    private void ZoomCamera(float input)
    {
        if (cameraTarget != null)
        {
            Vector3 offset = transform.position - cameraTarget.transform.position;
            offset += transform.forward * input * zoomSpeed * Time.deltaTime;
            transform.position = cameraTarget.transform.position + offset;
        }
    }

    private void RotateCamera(float input)
    {
        if (cameraTarget != null)
        {
            transform.RotateAround(cameraTarget.transform.position, Vector3.up, input * rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator Countdown()
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
