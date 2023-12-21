using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    public GameObject characterList;
    public GameObject cameraTarget;
    public float speed;
    
    // Start is called before the first frame update
    public void Awake()
    {
        characterList = GameObject.FindGameObjectWithTag("Player");
        cameraTarget = characterList.transform.Find("CameraTarget").gameObject;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        gameObject.transform.position = Vector3.Lerp(transform.position, cameraTarget.transform.position, Time.deltaTime * speed);
        gameObject.transform.LookAt(characterList.gameObject.transform.position);
    }
}
