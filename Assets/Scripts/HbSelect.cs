using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HbSelect : MonoBehaviour
{

    [Header("Overview")]
    [SerializeField] private int index;
    [SerializeField] private Text boardName;
    [SerializeField] private GameObject boardModel;

    [Header("Stats")]
    [SerializeField] private Text boardSpeed;
    [SerializeField] private Text boardBoost;
    [SerializeField] private Text boardHandling;
    [SerializeField] private Text boardWeightClass;

    [Header("Board Model")]
    [SerializeField] private Transform boardPosition;

    [Header("Menu Buttons")]
    [SerializeField] private GameObject select_Button;
    [SerializeField] private GameObject play_Button;

    private void Start()
    {
        index = PlayerPrefs.GetInt("CharacterSelected", 0);
    }

    public void DisplayBoard(Map map)
    {
        index = map.HbIndex;
        boardName.text = map.HbName;
        boardSpeed.text = map.HbSpeed;
        boardBoost.text = map.HbBoost;
        boardHandling.text = map.HbHandling;
        boardModel = map.boardModel;
        boardWeightClass.text = map.HbWeightClass;

        select_Button = map.selectButton;
        play_Button = map.selectButton;


        if (boardPosition.childCount > 0)
        {
            Destroy(boardPosition.GetChild(0).gameObject);
        }

        Instantiate(map.boardModel, boardPosition.position, boardPosition.rotation, boardPosition);

    }

    public void SelectBoard()
    {
        PlayerPrefs.SetInt("CharacterSelected", index);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("Level_1");
    }

}
