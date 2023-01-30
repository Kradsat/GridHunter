using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMovedGridPosition;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Transform gridDebugObjectPrefab;

    private GridSystem<GridObject> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("ther is more than ine LevelGrid" + transform + "_" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize,
                (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab, this.transform);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, UnitBase unit)
    {
        if (unit.Unit.Id != (int)MapData.OBJ_TYPE.BOSS && unit.Unit.Id != (int)MapData.OBJ_TYPE.BOSS2)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.AddUnit(unit);
            return;
        } else {
            GridPosition gridPos;
            gridPos.x = gridPosition.x - 1;
            gridPos.z = gridPosition.z - 1;
            for (int i = 0; i < 4; i++)
            {
                gridPosition.z = gridPos.z + i;
                for (int j = 0; j < 4; j++)
                {
                    gridPosition.x = gridPos.x + j;
                    GridObject gridObject = gridSystem.GetGridObject(gridPosition);
                    gridObject.AddUnit(unit);
                }
            }
        }
    }

    public List<UnitBase> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, UnitBase unit)
    {
        if (unit.Unit.Id != (int)MapData.OBJ_TYPE.BOSS && unit.Unit.Id != (int)MapData.OBJ_TYPE.BOSS2)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.RemoveUnit(unit);
            return;
        } else {
            GridPosition gridPos;
            gridPos.x = gridPosition.x - 1;
            gridPos.z = gridPosition.z - 1;
            for (int i = 0; i < 4; i++)
            {
                gridPosition.z = gridPos.z + i;
                for (int j = 0; j < 4; j++)
                {
                    gridPosition.x = gridPos.x + j;
                    GridObject gridObject = gridSystem.GetGridObject(gridPosition);
                    gridObject.RemoveUnit(unit);
                }
            }
        }
    }

    //Function that will be called whenever a unit changes its grid position
    public void UnitMovedGridPosition(UnitBase unit,GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        OnAnyUnitMovedGridPosition?.Invoke(this,EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();
    public float GetCellSize() => gridSystem.GetCellSize();


    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public UnitBase GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
}
