// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

using System.Collections.Generic;

public interface IMap
{
    ICell GetCell(int x, int y);
    IEnumerable<ICell> GetNeighbors(ICell cell);
}