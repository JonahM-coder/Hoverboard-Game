using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuCameraController : MonoBehaviour
{

    [Header("Menu Objects")]
    public GameObject currentMenu;
    public GameObject newMenu;

    [Header("Camera Object")]
    private Camera menuCamera;

    [Header("Buttons")]
    
    //Board Select Buttons
    public GameObject button1;
    
    //Return to Board Selection Button
    public GameObject button2;

    //Start Level Button
    public GameObject button3;

    [Header("Camera Object Transformation")]
    public Transform target;
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public float originalFOV = 15f;

    [Header("New Position Transformation")]
    public Vector3 newPosition;
    public Quaternion newRotation;
    public float newFOV = 15f;

    [Header("Camera Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 50f;

    [Header("Bool variables")]
    public bool isMoving = false;
    public bool isRotating = false;

    private float targetFOV;

    public void Start()
    {
        menuCamera = Camera.main;
        menuCamera.fieldOfView = Mathf.Clamp(menuCamera.fieldOfView, originalFOV, newFOV);

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        newPosition = target.position;
        newRotation = target.rotation;

        StartCoroutine(SmoothFOVChange());


        //Disable New Menu
        Text[] newMenuComponents = newMenu.GetComponentsInChildren<Text>();
        Button[] newMenuButtons = newMenu.GetComponentsInChildren<Button>();
        Image[] newMenuImages = newMenu.GetComponentsInChildren<Image>();

        foreach (Text textComponent in newMenuComponents)
        {
            textComponent.enabled = false;
        }

        foreach (Button buttonComponent in newMenuButtons)
        {
            buttonComponent.enabled = false;
        }

        foreach (Image imageComponent in newMenuImages)
        {
            imageComponent.enabled = false;
        }

    }

    // Update is called once per frame
    public void Update()
    {

        targetFOV = Mathf.Clamp(targetFOV, originalFOV, newFOV);

        if (isMoving)
        {
            Vector3 targetPosition = target.position;
            Vector3 currentPosition = transform.position;
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

            if (currentPosition == targetPosition)
            {
                isMoving = false;
            }

        }

        if (isRotating)
        {

            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            Quaternion currentRotation = transform.rotation;
            transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (currentRotation == targetRotation)
            {
                isRotating = false;
            }

        }

    }

    public void MoveCameraToTarget()
    {

        float currentFOV = menuCamera.fieldOfView - originalFOV;
        currentFOV = Mathf.Clamp(currentFOV, originalFOV, newFOV);
        targetFOV = currentFOV;
        menuCamera.fieldOfView = currentFOV;

        isMoving = true;
        isRotating = false;
        target.position = newPosition;
        target.rotation = newRotation;
        button1.SetActive(false);
        button2.SetActive(true);
        button3.SetActive(true);

    }

    public void ReturnCameraFromTarget()
    {

        float currentFOV = menuCamera.fieldOfView - newFOV;
        currentFOV = Mathf.Clamp(currentFOV, originalFOV, newFOV);
        targetFOV = currentFOV;
        menuCamera.fieldOfView = currentFOV;

        isMoving = true;
        isRotating = false;
        target.position = originalPosition;
        target.rotation = originalRotation;
        button3.SetActive(false);
        button2.SetActive(false);
        button1.SetActive(true);

    }

    private IEnumerator SmoothFOVChange()
    {
        float time = 0.2f;
        float currentTime = 0f;
        float startingFOV = menuCamera.fieldOfView;

        if (currentTime < time)
        {
            float t = currentTime / time;
            menuCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, t);

            currentTime += Time.deltaTime;
            yield return null;
        }

        menuCamera.fieldOfView = targetFOV;

    }

    public void ChangeMenu()
    {
        //Current Menu UI
        Text[] currentMenuComponents = currentMenu.GetComponentsInChildren<Text>();
        Button[] currentMenuButtons = currentMenu.GetComponentsInChildren<Button>();
        Image[] currentMenuImages = currentMenu.GetComponentsInChildren<Image>();

        //Next Menu UI
        Text[] newMenuComponents = newMenu.GetComponentsInChildren<Text>();
        Button[] newMenuButtons = newMenu.GetComponentsInChildren<Button>();
        Image[] newMenuImages = newMenu.GetComponentsInChildren<Image>();

        foreach (Text textComponent in currentMenuComponents)
        {
            textComponent.enabled = false;
        }

        foreach (Button buttonComponent in currentMenuButtons)
        {
            buttonComponent.enabled = false;
        }

        foreach (Image imageComponent in currentMenuImages)
        {
            imageComponent.enabled = false;
        }

        foreach (Text textComponent in newMenuComponents)
        {
            textComponent.enabled = true;
        }

        foreach (Button buttonComponent in newMenuButtons)
        {
            buttonComponent.enabled = true;
        }

        foreach (Image imageComponent in newMenuImages)
        {
            imageComponent.enabled = true;
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button3);

    }

    public void ChangeMenuBack()
    {
        //Current Menu UI
        Text[] currentMenuComponents = currentMenu.GetComponentsInChildren<Text>();
        Button[] currentMenuButtons = currentMenu.GetComponentsInChildren<Button>();
        Image[] currentMenuImages = currentMenu.GetComponentsInChildren<Image>();

        //Next Menu UI
        Text[] newMenuComponents = newMenu.GetComponentsInChildren<Text>();
        Button[] newMenuButtons = newMenu.GetComponentsInChildren<Button>();
        Image[] newMenuImages = newMenu.GetComponentsInChildren<Image>();

        foreach (Text textComponent in currentMenuComponents)
        {
            textComponent.enabled = true;
        }

        foreach (Button buttonComponent in currentMenuButtons)
        {
            buttonComponent.enabled = true;
        }

        foreach (Image imageComponent in currentMenuImages)
        {
            imageComponent.enabled = true;
        }

        foreach (Text textComponent in newMenuComponents)
        {
            textComponent.enabled = false;
        }

        foreach (Button buttonComponent in newMenuButtons)
        {
            buttonComponent.enabled = false;
        }

        foreach (Image imageComponent in newMenuImages)
        {
            imageComponent.enabled = false;
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button1);

    }

}
