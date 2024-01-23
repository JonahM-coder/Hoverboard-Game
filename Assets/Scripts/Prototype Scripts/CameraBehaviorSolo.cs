using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviorSolo : MonoBehaviour
{

    public GameObject player;
    public GameObject cameraTarget;
    
    public float speed;
    public string playerTag;

    // Start is called before the first frame update
    private void Awake()
    {

        FindPlayer();
        
        //player = GameObject.FindGameObjectWithTag("Player");
        //cameraTarget = player.transform.Find("CameraTarget").gameObject;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Follow();
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

}