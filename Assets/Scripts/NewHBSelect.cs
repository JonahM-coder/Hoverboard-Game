using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewHBSelect : MonoBehaviour
{
    private GameObject[] characterList;
    public int index = 0;

    public Text characterNameText;
    public Text speedText;
    public Text boostText;
    public Text turningText;

    private void Start()
    {
        

        
        index = PlayerPrefs.GetInt("CharacterSelected");

        characterList = new GameObject[transform.childCount];

        // Fill array with gameobjects
        for (int i = 0; i < transform.childCount; i++)
        {
            characterList[i] = transform.GetChild(i).gameObject;
        }

        // Toggle off their renderer
        foreach (GameObject go in characterList)
        {
            go.SetActive(false);
        }

        // Toggle on the selected character
        if (characterList[index])
        {
            characterList[index].SetActive(true);
        }

        // Set the initial character name
        UpdateCharacterInfo();
    }

    public void ToggleLeft()
    {
        characterList[index].SetActive(false);

        index--;

        if (index < 0)
        {
            index = characterList.Length - 1;
        }

        characterList[index].SetActive(true);

        // Update the character name text
        UpdateCharacterInfo();
    }

    public void ToggleRight()
    {
        characterList[index].SetActive(false);

        index++;

        if (index == characterList.Length)
        {
            index = 0;
        }

        characterList[index].SetActive(true);

        // Update the character name text
        UpdateCharacterInfo();
    }

    public void LoadScene()
    {
        PlayerPrefs.SetInt("CharacterSelected", index);
    }

    // Method to update the character name text
    private void UpdateCharacterInfo()
    {
        if (characterNameText != null && index >= 0 && index < characterList.Length)
        {
            characterNameText.text = characterList[index].name;

            // Assuming you have HoverboardAttributes component attached to each hoverboard
            HoverboardAttributes attributes = characterList[index].GetComponent<HoverboardAttributes>();
            if (attributes != null)
            {
                // Update speed, boost, and turning text
                speedText.text = attributes.Speed.ToString();
                boostText.text = attributes.Boost.ToString();
                turningText.text = attributes.Turning.ToString();
            }
        }
    }
}
