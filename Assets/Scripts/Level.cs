using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Scriptable Objects/Level")]

public class Level : ScriptableObject
{

    [Header("Overview")]
    public int levelIndex;
    public string areaName;
    public GameObject areaModel;

    [Header("Level Name")]
    public string levelName;
    public string difficultyText;
    public GameObject levelModel;

    [Header("Scene change")]
    public Object sceneToLoad;

}
