using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewLevelSelect : MonoBehaviour
{
    public string[] sceneList;
    public int index = 2;
    public Text sceneNameText;

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
        sceneNameText.text = sceneList[index]; // Update the UI Text with the current scene name
    }

}
