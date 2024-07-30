// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

using UnityEngine;

public class CameraResizer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private HexGrid grid;
    [SerializeField] private float buffer = 1.0f; // Buffer to ensure grid edges are not cut off

    private void Start()
    {
        CenterCamera();
        AdjustCameraSize();
    }

    private void CenterCamera()
    {
        var width = grid.Width * grid.HexSizeX;
        var height = grid.Height * grid.HexSizeY;

        mainCamera.transform.position = new Vector3(width * 0.5f, height * 0.5f, mainCamera.transform.position.z);
    }

    private void AdjustCameraSize()
    {
        float gridWidth = grid.Width * grid.HexSizeX;
        float gridHeight = grid.Height * grid.HexSizeY;

        float aspectRatio = Screen.width / (float)Screen.height;
        float verticalSize = (gridHeight * 0.5f) + buffer;
        float horizontalSize = ((gridWidth * 0.5f) + buffer) / aspectRatio;

        // Set the orthographic size to the larger of the two values
        mainCamera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
    }
}