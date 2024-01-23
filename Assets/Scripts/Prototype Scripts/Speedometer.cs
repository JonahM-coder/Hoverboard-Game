using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{

    public HbController controller;
    public Text currentSpeed;

    // Update is called once per frame
    private void Update()
    {
        if(controller != null)
        {
            float speed = controller.GetSpeed();

            currentSpeed.text = "Speed: " + speed.ToString("F1") + " m/s";
        }
    }
}
