// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

public class HexCell : ICell
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsWalkable { get; set; }

    public HexCell(int x, int y, bool isWalkable)
    {
        X = x;
        Y = y;
        IsWalkable = isWalkable;
    }
}