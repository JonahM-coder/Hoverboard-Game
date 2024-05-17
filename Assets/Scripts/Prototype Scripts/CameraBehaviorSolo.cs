using UnityEngine;

public class CameraBehaviorSolo : MonoBehaviour
{
    public Transform[] cameraTargets; // Array to hold multiple camera targets
    public float positionLerpSpeed = 500f; // Speed at which the camera follows the position
    public float rotationLerpSpeed = 300f; // Speed at which the camera follows the rotation
    private int currentTargetIndex = 0; // Get index from PlayerPref


    void Start()
    {
        // Ensure we have camera targets set
        if (cameraTargets.Length == 0)
        {
            Debug.LogWarning("No camera targets set. Please assign camera targets.");
            return;
        }

        // Load the current target index from PlayerPrefs
        currentTargetIndex = PlayerPrefs.GetInt("CharacterSelected");

        // Ensure the loaded index is valid
        if (currentTargetIndex < 0 || currentTargetIndex >= cameraTargets.Length)
        {
            Debug.LogWarning("Loaded target index is out of range. Resetting to 0.");
            currentTargetIndex = 0;
        }

        Debug.Log("Starting with camera target index: " + currentTargetIndex);

        // Initialize the camera position and rotation to match the initial target
        if (cameraTargets[currentTargetIndex] != null)
        {
            Vector3 targetPosition = cameraTargets[currentTargetIndex].position;
            transform.position = cameraTargets[currentTargetIndex].position;
            transform.rotation = cameraTargets[currentTargetIndex].rotation;
        }
        else
        {
            Debug.LogWarning("Initial camera target is null.");
        }
    }

    void LateUpdate()
    {
        if (cameraTargets.Length == 0 || cameraTargets[currentTargetIndex] == null) return;

        // Smoothly interpolate the position and rotation of the camera to follow the current target
        Vector3 targetPosition = cameraTargets[currentTargetIndex].position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, positionLerpSpeed * Time.deltaTime);

        Quaternion targetRotation = cameraTargets[currentTargetIndex].rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
    }


}
