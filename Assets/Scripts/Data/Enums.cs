// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

public class Enums
{
    public enum CellDirection
    {
        Left,
        Right,
        DownLeft,
        DownRight,
        UpLeft,
        UpRight
    }

    public enum CellVisualState
    {
        Walkable,
        Blocked,
        StartPoint,
        EndPoint,
        PathPoint
    }
}