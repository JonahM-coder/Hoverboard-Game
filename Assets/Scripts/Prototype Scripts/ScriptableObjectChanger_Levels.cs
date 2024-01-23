using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectChanger_Levels : MonoBehaviour
{

    [SerializeField] private ScriptableObject[] serializableObjects;

    [SerializeField] private LevelSelect levelSelector;

    private int currentIndex;

    private void Awake()
    {
        ChangeScriptableLevelObject(0);
    }

    public void ChangeScriptableLevelObject(int change)
    {
        currentIndex += change;

        if (currentIndex < 0)
        {
            currentIndex = serializableObjects.Length - 1;
        }
        else if (currentIndex > serializableObjects.Length - 1)
        {
            currentIndex = 0;
        }

        if (levelSelector != null)
        {
            levelSelector.DisplayLevel((Level)serializableObjects[currentIndex]);
        }
    }

}
