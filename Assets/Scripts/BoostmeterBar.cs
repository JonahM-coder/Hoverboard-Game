using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostmeterBar : MonoBehaviour
{

    public Slider slider;
    public Gradient barColor;
    public Image fill;

    //Our start function for our boost meter bar script
    public void SetMaxMeter(float size)
    {
        slider.maxValue = size;
        slider.value = size;

        fill.color = barColor.Evaluate(10000f);
    }

    public void SetMeter(float size)
    {
        slider.value = size;

        fill.color = barColor.Evaluate(slider.normalizedValue);
    }

}
