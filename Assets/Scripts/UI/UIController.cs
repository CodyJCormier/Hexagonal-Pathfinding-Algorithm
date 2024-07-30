// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Using Text Mesh Pro as it's in base Unity, and is always recommended over the legacy Text, except in the most extreme scenarios, though this required an import.
    [SerializeField] private TextMeshProUGUI startCoordinatesText;
    [SerializeField] private TextMeshProUGUI endCoordinatesText;
    [SerializeField] private TextMeshProUGUI pathLengthText;
    [SerializeField] private Button reloadButton;

    private void Awake()
    {
        if(!reloadButton)
            this.Log("Error_1(Missing Ref):missing reloadButton scene reference");

        reloadButton.onClick.AddListener(ReloadScene);
    }

    public void UpdateStartCoordinates(int x, int y)
    {
        if(!startCoordinatesText)
            this.Log("Error_1(Missing Ref):missing startCoordinatesText scene reference");

        startCoordinatesText.text = $"Start: ({x}, {y})";
    }

    public void ResetStartCell()
    {
        if(!startCoordinatesText)
            this.Log("Error_1(Missing Ref):missing startCoordinatesText scene reference");

        startCoordinatesText.text = $"Start: (-, -)";
    }

    public void UpdateEndCoordinates(int x, int y)
    {
        if(!endCoordinatesText)
            this.Log("Error_1(Missing Ref):missing endCoordinatesText scene reference");

        endCoordinatesText.text = $"End: ({x}, {y})";
        ClearPath();
    }

    public void ResetEndCell()
    {
        if(!startCoordinatesText)
            this.Log("Error_1(Missing Ref):missing endCoordinatesText scene reference");

        startCoordinatesText.text = $"Start: (-, -)";
        ClearPath();
    }

    private void ClearPath()
    {
        if(!pathLengthText)
            this.Log("Error_1(Missing Ref):missing pathLengthText scene reference");

        pathLengthText.text = $"Path Length: -";
    }

    public void UpdatePathLength(int length)
    {
        if(!pathLengthText)
            this.Log("Error_1(Missing Ref):missing pathLengthText scene reference");

        pathLengthText.text = $"Path Length: {length}";
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}