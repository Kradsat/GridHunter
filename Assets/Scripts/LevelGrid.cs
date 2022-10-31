using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMovedGridPosition;

    [SerializeField] private int GridSize;
    [SerializeField] private float cellSize;
    [SerializeField] private Transform gridDebugObjectPrefab;

    private GridSystem gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("ther is more than ine LevelGrid" + transform + "_" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem(GridSize, GridSize, cellSize);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab, this.transform);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        if( !unit.is3x3Boss && !unit.is2x1Enemy ) {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.AddUnit(unit);
        } else if( unit.is2x1Enemy ) {
            if( unit.transform.rotation.y >= -44.99 && unit.transform.rotation.y <= 44.99 ||
                 unit.transform.rotation.y >= 315 || unit.transform.rotation.y <= -315 ) {
                for( int i = 0; i < 2; i++ ) {
                    gridPosition.z += i;
                    GridObject gridObject = gridSystem.GetGridObject( gridPosition );
                    gridObject.AddUnit( unit );
                }
            } else if( unit.transform.rotation.y >= 45 && unit.transform.rotation.y <= 134.99 ||
                       unit.transform.rotation.y >= -314.99 && unit.transform.rotation.y <= -225 ) {
                for( int i = 0; i < 2; i++ ) {
                    gridPosition.x += i;
                    GridObject gridObject = gridSystem.GetGridObject( gridPosition );
                    gridObject.AddUnit( unit );
                }
            } else if( unit.transform.rotation.y >= 225 && unit.transform.rotation.y <= 314.99 ||
                       unit.transform.rotation.y >= -134.99 && unit.transform.rotation.y <= -45 ) {
                for( int i = 0; i < 2; i++ ) {
                    gridPosition.x -= i;
                    GridObject gridObject = gridSystem.GetGridObject( gridPosition );
                    gridObject.AddUnit( unit );
                }
            } else if( unit.transform.rotation.y >= 135 && unit.transform.rotation.y <= 224.99 ||
                       unit.transform.rotation.y >= -135 && unit.transform.rotation.y <= -224.99 ) {
                for( int i = 0; i < 2; i++ ) {
                    gridPosition.z -= i;
                    GridObject gridObject = gridSystem.GetGridObject( gridPosition );
                    gridObject.AddUnit( unit );
                }
            }
        } else {
            GridPosition gridPos;
            gridPos.x = gridPosition.x - 1;
            gridPos.z = gridPosition.z - 1;
            for( int i = 0; i < 4; i++ ) {
                gridPosition.z = gridPos.z + i;
                for( int j = 0; j < 3; j++ ) {
                    gridPosition.x = gridPos.x + j;
                    GridObject gridObject = gridSystem.GetGridObject( gridPosition );
                    gridObject.AddUnit( unit );
                }
            }
        }
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        if( !unit.is3x3Boss && !unit.is2x1Enemy ) {
            GridObject gridObject = gridSystem.GetGridObject( gridPosition );
            gridObject.RemoveUnit( unit );
        } else if( unit.is2x1Enemy ) {
            if( unit.transform.rotation.y >= -44.99 && unit.transform.rotation.y <= 44.99 ||
                 unit.transform.rotation.y >= 315 || unit.transform.rotation.y <= -315 ) {
                for( int i = 0; i < 2; i++ ) {
                    gridPosition.z += i;
                    GridObject gridObject = gridSystem.GetGridObject( gridPosition );
                    gridObject.RemoveUnit( unit );
                }
            } else if( unit.transform.rotation.y >= 45 && unit.transform.rotation.y <= 134.99 ||
                       unit.transform.rotation.y >= -314.99 && unit.transform.rotation.y <= -225 ) {
                for( int i = 0; i < 2; i++ ) {
                    gridPosition.x += i;
                    GridObject gridObject = gridSystem.GetGridObject( gridPosition );
                    gridObject.RemoveUnit( unit );
                }
            } else if( unit.transform.rotation.y >= 225 && unit.transform.rotation.y <= 314.99 ||
                       unit.transform.rotation.y >= -134.99 && unit.transform.rotation.y <= -45 ) {
                for( int i = 0; i < 2; i++ ) {
                    gridPosition.x -= i;
                    GridObject gridObject = gridSystem.GetGridObject( gridPosition );
                    gridObject.RemoveUnit( unit );
                }
            } else if( unit.transform.rotation.y >= 135 && unit.transform.rotation.y <= 224.99 ||
                       unit.transform.rotation.y >= -135 && unit.transform.rotation.y <= -224.99 ) {
                for( int i = 0; i < 2; i++ ) {
                    gridPosition.z -= i;
                    GridObject gridObject = gridSystem.GetGridObject( gridPosition );
                    gridObject.RemoveUnit( unit );
                }
            }
        } else {
            GridPosition gridPos;
            gridPos.x = gridPosition.x - 1;
            gridPos.z = gridPosition.z - 1;
            for( int i = 0; i < 4; i++ ) {
                gridPosition.z = gridPos.z + i;
                for( int j = 0; j < 3; j++ ) {
                    gridPosition.x = gridPos.x + j;
                    GridObject gridObject = gridSystem.GetGridObject( gridPosition );
                    gridObject.RemoveUnit( unit );
                }
            }
        }
    }

    //Function that will be called whenever a unit changes its grid position
    public void UnitMovedGridPosition(Unit unit,GridPosition fromGridPosition, GridPosition toGridPosition)
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

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
}
