// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

using TMPro;
using UnityEngine;
using CellVisualState = Enums.CellVisualState;

public class HexCellView : MonoBehaviour
{
    public HexCell Cell { get; private set; }
    [SerializeField] private Renderer cellRenderer;
    [SerializeField] private TextMeshProUGUI cellCoordinatesText;

    public void SetCell(HexCell cellToSet)
    {
        Cell = cellToSet;
        cellCoordinatesText.text = $"{Cell.X}, {Cell.Y}";
        UpdateWalkabilityVisual();
    }

    private void UpdateWalkabilityVisual()
    {
        // Update the visual representation of the cell, e.g., color based on walkability
        if(!cellRenderer)
            this.Log("Error_1(Missing Ref):missing cellRenderer scene reference");

        SetPointState(Cell.IsWalkable ? CellVisualState.Walkable : CellVisualState.Blocked);
    }

    public void SetPointState(CellVisualState currentState)
    {
        cellRenderer.material.color = Tools.GetColorState(currentState);
    }

    public void RemoveSpecialState()
    {
        UpdateWalkabilityVisual();
    }
}