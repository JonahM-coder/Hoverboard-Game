using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewLevelSelect : MonoBehaviour
{
    public string[] sceneList;
    public int index = 0;
    public Text planetNameText;
    public Text levelNameText;
    public Text difficultyText;

    public string[] planetNames;
    public string[] levelNames;
    public string[] difficultyNames;

    public GameObject[] planetObjects;

    private void Start()
    {
        index = PlayerPrefs.GetInt("SceneSelected", 0); // Default to the first scene
        UpdateSceneNameText();
    }

    public void ToggleLeft()
    {
        ChangeScene(-1);
        UpdateSceneNameText();
    }

    public void ToggleRight()
    {
        ChangeScene(1);
        UpdateSceneNameText();
    }

    private void ChangeScene(int direction)
    {
        index = (index + direction + sceneList.Length) % sceneList.Length;
    }

    public void LoadScene()
    {
        PlayerPrefs.SetInt("SceneSelected", index);
        SceneManager.LoadScene(sceneList[index], LoadSceneMode.Single);
    }

    private void UpdateSceneNameText()
    {
        // Update the UI Text with the current scene name
        planetNameText.text = planetNames[index];
        levelNameText.text = levelNames[index];
        difficultyText.text = difficultyNames[index];

        foreach (GameObject planetObj in planetObjects)
        {
            planetObj.SetActive(false);
        }

        if (index < planetObjects.Length)
        {
            planetObjects[index].SetActive(true);
        }

    }

}
