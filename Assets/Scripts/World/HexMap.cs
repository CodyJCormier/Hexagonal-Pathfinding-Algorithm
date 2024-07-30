// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

using System.Collections.Generic;
using UnityEngine;

public class HexMap : IMap
{
    private readonly HexCell[,] cells;
    private readonly int width;
    private readonly int height;

    public HexMap(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new HexCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool isWalkable = Random.Range(0, 100) >= 15; // 85% chance for walkable cells
                cells[x, y] = new HexCell(x, y, isWalkable);
            }
        }
    }

    public ICell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return cells[x, y];
        }
        return null;
    }

    public IEnumerable<ICell> GetNeighbors(ICell cell)
    {
        var neighbors = new List<ICell>();
        var directions = Tools.GetHexCellDirections(cell.Y);

        foreach (var direction in directions)
        {
            int nx = cell.X + direction[0];
            int ny = cell.Y + direction[1];
            ICell neighbor = GetCell(nx, ny);
            if (neighbor != null && neighbor.IsWalkable)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}