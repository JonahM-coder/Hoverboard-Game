using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "New Map", menuName = "Scriptable Objects/Map")]

public class Map : ScriptableObject
{
    
    [Header("Overview")]
    public int HbIndex;
    public GameObject boardModel;
    public string HbName;

    [Header("Player")]
    public GameObject hbPlayer;

    [Header("Stats")]   
    public string HbSpeed;
    public string HbBoost;
    public string HbHandling;
    public string HbWeightClass;

    [Header("Stat Colors")]
    public Color speedColor;
    public Color boostColor;
    public Color handlingColor;

    [Header("Menu Buttons")]
    public GameObject selectButton;
    public GameObject playButton;

    [Header("Scene change")]
    public Object sceneToLoad;

}
