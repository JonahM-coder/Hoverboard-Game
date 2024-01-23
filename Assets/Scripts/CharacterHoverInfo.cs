using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHoverInfo : MonoBehaviour
{
    public Text hoverText; // Reference to the UI text element

    private void Start()
    {
        hoverText.gameObject.SetActive(false); // Initially, hide the hover text
    }

    private void OnMouseEnter()
    {
        // Display information when the mouse enters the object
        UpdateHoverText();
        hoverText.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        // Hide information when the mouse exits the object
        hoverText.gameObject.SetActive(false);
    }

    private void UpdateHoverText()
    {
        // Customize the hover text content based on the currently selected character
        // You can modify this based on your game's requirements
        int selectedCharacterIndex = GetComponent<NewHBSelect>().index;
        string characterName = GetCharacterName(selectedCharacterIndex);

        hoverText.text = "Selected Character: " + characterName;
    }

    // Add more character names or information as needed
    private string GetCharacterName(int index)
    {
        // Example: You may have an array of character names
        string[] characterNames = { "Character1", "Character2", "Character3" };

        // Ensure the index is within bounds
        if (index >= 0 && index < characterNames.Length)
        {
            return characterNames[index];
        }

        return "Unknown Character";
    }
}

