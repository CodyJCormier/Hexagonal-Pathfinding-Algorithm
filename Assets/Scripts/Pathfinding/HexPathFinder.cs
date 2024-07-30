// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexPathFinder : IPathFinder
{
    private readonly HexMap _hexMap;
    private readonly HexCellView[,] _hexCellView;

    public HexPathFinder(HexMap map, HexCellView[,] hexCellViews)
    {
        _hexMap = map;
        _hexCellView = hexCellViews;
    }

    private ICell GetLowestFScore(List<ICell> openSet, Dictionary<ICell, int> fScore)
    {
        return openSet.OrderBy(cell => fScore[cell]).First();
    }

    private int Heuristic(ICell a, ICell b)
    {
        int dx = Mathf.Abs(a.X - b.X);
        int dy = Mathf.Abs(a.Y - b.Y);
        int dz = Mathf.Abs((a.X - a.Y) - (b.X - b.Y));
        return Mathf.Max(dx, dy, dz);
    }

    private int Distance(ICell a, ICell b)
    {
        return Heuristic(a, b);
    }

    private List<ICell> ReconstructPath(Dictionary<ICell, ICell> cameFrom, ICell current)
    {
        var totalPath = new List<ICell> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        totalPath.Reverse();
        Debug.Log("Path reconstructed: " + string.Join(" -> ", totalPath.Select(cell => $"({cell.X},{cell.Y})")));
        return totalPath;
    }

    public IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd, IMap map)
    {
        var openSet = new List<ICell> { cellStart };
        var closedSet = new HashSet<ICell>();
        var cameFrom = new Dictionary<ICell, ICell>();
        var gScore = new Dictionary<ICell, int> { [cellStart] = 0 };
        var fScore = new Dictionary<ICell, int> { [cellStart] = Heuristic(cellStart, cellEnd) };

        while (openSet.Count > 0)
        {
            var current = GetLowestFScore(openSet, fScore);

            if (current.Equals(cellEnd))
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (ICell neighbor in _hexMap.GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                int tentativeGScore = gScore[current] + Distance(current, neighbor);

                if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, cellEnd);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return new List<ICell>(); // Return an empty path if no path is found
    }
}