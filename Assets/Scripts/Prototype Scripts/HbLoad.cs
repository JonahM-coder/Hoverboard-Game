using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HbLoad : MonoBehaviour
{

    public Map[] boardSelectMenu;
    public Map selectedBoard;
    private int selectedIndex = -1;

    public void SelectBoard(int index)
    {
        selectedIndex = index;

        if (selectedIndex >= 0 && selectedIndex < boardSelectMenu.Length)
        {
            selectedBoard = boardSelectMenu[selectedIndex];
        }

    }

}
