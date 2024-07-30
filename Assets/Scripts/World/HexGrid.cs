// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

using System.Collections.Generic;
using UnityEngine;
using CellVisualState = Enums.CellVisualState;

public class HexGrid : MonoBehaviour
{
    [Header("Settings")]
    [Range(1, 100)] [field:SerializeField]
    public int Width
    {
        get; private set;
    }

    [Range(1, 100)] [field:SerializeField] public int Height
    {
        get; private set;
    }

    [Header("Data")]
    [SerializeField] private GameObject hexCellPrefab;
    [Header("SceneReferences")]
    [SerializeField] private UIController uiController;
    [SerializeField] private Camera mainCamera;

    public float HexSizeX { get; private set; }
    public float HexSizeY { get; private set; }

    private HexMap _hexMap;
    private HexCellView[,] _hexCellViews;
    private List<HexCellView> _currentPath;
    private ICell _startCell;
    private ICell _endCell;
    private IPathFinder _pathFinder;
    private bool _useTouch;

    private void Awake()
    {
        _useTouch = Input.touchSupported;
        _hexMap = new HexMap(Width, Height);
        _hexCellViews = new HexCellView[Width, Height];

        if (!uiController)
            uiController = FindFirstObjectByType<UIController>();

        HexSizeX = hexCellPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        HexSizeY = hexCellPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var cell = _hexMap.GetCell(x, y) as HexCell;
                if (cell == null)
                {
                    Debug.Log($"Map did not have cell at ({x}, {y})");
                    continue;
                }

                var shouldOffset = (y % 2) == 0;
                float offset = (shouldOffset) ? HexSizeX * 0.5f : 0;

                var xPosition = (x * HexSizeX) + offset;
                var yPosition = y * HexSizeY * 0.75f;
                var hexCellObject = Instantiate(hexCellPrefab, transform);
                hexCellObject.transform.SetPositionAndRotation(new Vector3(xPosition, yPosition, 0), Quaternion.identity);
                hexCellObject.name = $"HexCell_{x}_{y}";
                var hexCellView = hexCellObject.GetComponent<HexCellView>();
                hexCellView.SetCell(cell);
                _hexCellViews[x, y] = hexCellView;
            }
        }

        _pathFinder = new HexPathFinder(_hexMap, _hexCellViews);
    }

    private void Update()
    {
        if (_useTouch ? Input.touchCount == 0 : !Input.GetMouseButtonDown(0))
            return;

        Vector3 screenPosition = _useTouch ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(screenPosition);

        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        Debug.DrawRay(mainCamera.transform.position, worldPoint - mainCamera.transform.position, Color.red, 10f);

        if (hit.transform == null)
            return;

        ICell clickedCell = hit.transform.GetComponent<HexCellView>()?.Cell;

        if (clickedCell is not { IsWalkable: true })
            return;

        if (_startCell == null && clickedCell != _endCell)
        {
            _startCell = clickedCell;
            _hexCellViews[clickedCell.X, clickedCell.Y].SetPointState(CellVisualState.StartPoint);
            uiController.UpdateStartCoordinates(_startCell.X, _startCell.Y);
        }
        else if (clickedCell == _startCell)
        {
            _startCell = null;
            _hexCellViews[clickedCell.X, clickedCell.Y].RemoveSpecialState();
            uiController.ResetStartCell();
            ResetPathCellsIfValid();
        }
        else if (_endCell == null && clickedCell != _startCell)
        {
            _endCell = clickedCell;
            _hexCellViews[clickedCell.X, clickedCell.Y].SetPointState(CellVisualState.EndPoint);
        }
        else if (clickedCell == _endCell)
        {
            _endCell = null;
            _hexCellViews[clickedCell.X, clickedCell.Y].RemoveSpecialState();
            uiController.ResetEndCell();
            ResetPathCellsIfValid();
        }

        if (_endCell != null && _startCell != null)
            GeneratePathAndSetVisuals();
    }

    private void GeneratePathAndSetVisuals()
    {
        var path = _pathFinder.FindPathOnMap(_startCell, _endCell, _hexMap);

        uiController.UpdateEndCoordinates(_endCell.X, _endCell.Y);
        uiController.UpdatePathLength(path.Count);

        ResetPathCellsIfValid();

        foreach (var currentCell in path)
        {
            if (currentCell == null || currentCell == _startCell || currentCell == _endCell) continue;
            _currentPath.Add(_hexCellViews[currentCell.X, currentCell.Y]);
            _hexCellViews[currentCell.X, currentCell.Y].SetPointState(CellVisualState.PathPoint);
        }
    }

    private void ResetPathCellsIfValid()
    {
        if (_currentPath != null && _currentPath.Count > 0)
        {
            foreach (var cellView in _currentPath)
            {
                cellView.RemoveSpecialState();
            }
            _currentPath.Clear();
        }

        _currentPath = new List<HexCellView>();
    }

    private Vector2Int ConvertWorldPointToCellPoint(Vector3 worldPoint)
    {
        worldPoint.z = 0;

        float col = (worldPoint.x * Mathf.Sqrt(3) / 3 - worldPoint.y / 3) / HexSizeX;
        float row = (worldPoint.y * 2 / 3) / HexSizeY;

        int x = Mathf.RoundToInt(col);
        int y = Mathf.RoundToInt(row);

        bool shouldOffset = (y % 2) == 0;
        float offset = (shouldOffset) ? HexSizeX * 0.5f : 0;

        float xPosition = (x * HexSizeX * Mathf.Sqrt(3)) + offset;
        float yPosition = y * (HexSizeY * 0.75f);

        return new Vector2Int(Mathf.RoundToInt((xPosition - offset) / (HexSizeX * Mathf.Sqrt(3))), Mathf.RoundToInt(yPosition / (HexSizeY * 0.75f)));
    }
}