// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

using System.Collections.Generic;

interface IPathFinder
{
    IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd, IMap map);
}