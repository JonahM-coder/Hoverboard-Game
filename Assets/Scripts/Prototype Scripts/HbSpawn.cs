using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HbSpawn : MonoBehaviour
{

    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        string selectedPrefabName = PlayerPrefs.GetString("SelectedPrefab");
        Debug.Log("Selected prefab name: " + selectedPrefabName);

        GameObject selectedPrefab = Resources.Load<GameObject>(selectedPrefabName); //Requires a folder named "Resources" for objects loading in between scenes

        if (selectedPrefab != null)
        {
            Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Object spawned");
        }
        else
        {
            Debug.Log("Object spawn logic finished");
        }

        PlayerPrefs.DeleteKey("SelectedPrefab");

    }

}
