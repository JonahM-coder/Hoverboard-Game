using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{

    public GameObject playerCollection;
    public Text speedometerText;

    // Update is called once per frame
    private void Update()
    {
        NewHbController controller = playerCollection.GetComponentInChildren<NewHbController>();

        if (controller != null)
        {
            float speed = controller.GetSpeed();

            speedometerText.text = "Speed: " + speed.ToString("F1") + " m/s";
        }

    }
}
