using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRotation : MonoBehaviour
{

    public GameObject[] buttonGameObjects;
    private int currentButtonIndex = 0;

    private void Start()
    {
        buttonGameObjects = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            buttonGameObjects[i] = transform.GetChild(i).gameObject;
        }

        ActivateButtonAtIndex(currentButtonIndex);
    }

    private void ActivateButtonAtIndex(int index)
    {
        for (int i = 0; i < buttonGameObjects.Length; i++)
        {
            buttonGameObjects[i].SetActive(i == index);
        }
    }

    public void NextButton()
    {
        currentButtonIndex = (currentButtonIndex + 1) % buttonGameObjects.Length;
        ActivateButtonAtIndex(currentButtonIndex);
    }

    public void PreviousButton()
    {
        currentButtonIndex = (currentButtonIndex - 1 + buttonGameObjects.Length) % buttonGameObjects.Length;
        ActivateButtonAtIndex(currentButtonIndex);
    }

    public void ShowButtonSet(int setIndex)
    {
        for (int i = 0; i < buttonGameObjects.Length; i++)
        {
            buttonGameObjects[i].SetActive(i == setIndex);
        }
        currentButtonIndex = setIndex;
    }


}
