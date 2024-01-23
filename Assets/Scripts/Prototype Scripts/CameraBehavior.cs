using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject[] characterList;
    public GameObject[] cameraTargetList;
    private Dictionary<GameObject, GameObject> characterCameraPairs = new Dictionary<GameObject, GameObject>();
    private GameObject activeCharacter;
    private GameObject activeCameraTarget;
    public float speed = 500f;
    public float rotationSpeed = 100f;

    public int index = 0;

    private void Awake()
    {
        InitializeCharacters();
        SelectManuallyAssignedPair(index); // Set the initial active character (change this index as needed)
    }

    private void FixedUpdate()
    {
        if (activeCameraTarget != null)
        {
            Follow();
        }
    }

    private void InitializeCharacters()
    {
        characterList = GameObject.FindGameObjectsWithTag("GameController");
        cameraTargetList = GameObject.FindGameObjectsWithTag("CameraTag");
    }

    private void SelectManuallyAssignedPair(int index)
    {
        if (index >= 0 && index < characterList.Length && index < cameraTargetList.Length)
        {
            activeCharacter = characterList[index];
            activeCameraTarget = cameraTargetList[index];
        }
        else
        {
            Debug.LogWarning("Invalid pair index or insufficient character/camera targets.");
        }
    }

    private void Follow()
    {
        
    }

}
