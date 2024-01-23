using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCameras : MonoBehaviour
{

    public Camera mainCamera;
    public Camera goalCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera.enabled = true;
        goalCamera.enabled = false;
    }

}
