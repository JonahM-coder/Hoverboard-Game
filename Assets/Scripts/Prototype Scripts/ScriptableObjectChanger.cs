using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectChanger : MonoBehaviour
{

    [SerializeField] private ScriptableObject[] serializableObjects;

    [SerializeField] private HbSelect boardSelector;

    private int currentIndex;

    private void Awake()
    {
        ChangeScriptableObject(0);
    }

    public void ChangeScriptableObject(int change)
    {
        currentIndex += change;

        if(currentIndex < 0)
        {
            currentIndex = serializableObjects.Length - 1;
        }
        else if(currentIndex > serializableObjects.Length - 1)
        {
            currentIndex = 0;
        }

        if (boardSelector != null)
        {
            boardSelector.DisplayBoard((Map)serializableObjects[currentIndex]);
        }
    }
}
