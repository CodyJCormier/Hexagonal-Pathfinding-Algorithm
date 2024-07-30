// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

using System;
using UnityEngine;
using CellVisualState = Enums.CellVisualState;

public static class Tools
{
    public static string ConvertCellDirectionToReadableString(int x, int y)
    {
        return (x, y) switch
        {
            (1, 0) => Enums.CellDirection.Left.ToString(),
            (-1, 0) => Enums.CellDirection.Right.ToString(),
            (1, 1) => Enums.CellDirection.UpRight.ToString(),
            (-1, -1) => Enums.CellDirection.DownLeft.ToString(),
            (1, -1) => Enums.CellDirection.UpLeft.ToString(),
            (-1, 1) => Enums.CellDirection.DownRight.ToString(),
            _ => $"Incorrect grid direction ({x}, {y})"
        };
    }

    public static Color GetColorState(CellVisualState visualState)
    {
        return visualState switch
        {
            CellVisualState.Walkable => Color.white,
            CellVisualState.Blocked => Color.black,
            CellVisualState.StartPoint => Color.yellow,
            CellVisualState.EndPoint => Color.red,
            CellVisualState.PathPoint => Color.blue,
            _ => throw new ArgumentOutOfRangeException(nameof(visualState), visualState, null),
        };
    }

    public static readonly int[][] HexCellDirectionsEven =
    {
        new[] { 1, 0 }, new[] { 0, -1 }, new[] { -1, -1 },
        new[] { -1, 0 }, new[] { -1, 1 }, new[] { 0, 1 }
    };

    public static readonly int[][] HexCellDirectionsOdd =
    {
        new[] { 1, 0 }, new[] { 1, -1 }, new[] { 0, -1 },
        new[] { -1, 0 }, new[] { 0, 1 }, new[] { 1, 1 }
    };

    public static int[][] GetHexCellDirections(int row)
    {
        return (row % 2 == 0) ? HexCellDirectionsOdd : HexCellDirectionsEven;
    }
}