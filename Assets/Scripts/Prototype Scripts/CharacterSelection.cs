using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] characterList;
    private int index = 0;

    private void Start()
    {
        index = PlayerPrefs.GetInt("CharacterSelected", 0); // Default to first character

        characterList = new GameObject[transform.childCount];

        //Fill array with gameobjects
        for (int i = 0; i < transform.childCount; i++)
        {
            characterList[i] = transform.GetChild(i).gameObject;
        }

        //Toggle off their renderer
        foreach (GameObject go in characterList)
        {
            go.SetActive(false);
        }

        //Toggle on the selected character
        if (characterList[index])
        {
            characterList[index].SetActive(true);
        }
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
    }

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("CharacterSelected", index);
    }

}
