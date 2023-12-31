﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviorSolo : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraTarget;
    public float speed = 500f;

    // Start is called before the first frame update
    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraTarget = player.transform.Find("CameraTarget").gameObject;
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
}
