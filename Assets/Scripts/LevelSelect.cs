using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelSelect : MonoBehaviour
{
    [Header("Overview")]
    [SerializeField] private Text areaName;
    [SerializeField] private Text levelName;
    [SerializeField] private Text difficultyText;

    [Header("Level Select Table Components")]
    [SerializeField] private GameObject areaModel;
    [SerializeField] private GameObject levelModel;

    [SerializeField] private Transform areaModelPosition;
    [SerializeField] private Transform levelModelPosition;

    public void DisplayLevel(Level lvl)
    {

        areaName.text = lvl.areaName;
        levelName.text = lvl.levelName;
        difficultyText.text = lvl.difficultyText;
        
        //Area model position details
        if (areaModelPosition.childCount > 0)
        {
            Destroy(areaModelPosition.GetChild(0).gameObject);
        }

        Instantiate(lvl.areaModel, areaModelPosition.position, areaModelPosition.rotation, areaModelPosition);

        //Level model position details
        if (levelModelPosition.childCount > 0)
        {
            Destroy(levelModelPosition.GetChild(0).gameObject);
        }

        Instantiate(lvl.levelModel, levelModelPosition.position, levelModelPosition.rotation, levelModelPosition);

    }
}
