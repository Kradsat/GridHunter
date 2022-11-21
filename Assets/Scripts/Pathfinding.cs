using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; set; }

    /// <summary>
    /// Move cost should be 1 and 1.4(sqr(2)) but,
    /// In order to make the code and calculation a bit simplier 1 and 1.4 were multilpied by 10
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14; // for this specific project we were asked not to implemment the diagonal movement, but i will leave it as a comment for future referenses

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask nonWalkableLayer;

    GridObject gridObject;

    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one Pathfinding! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Setup(int width, int height, float cellSize)
    {
        Debug.Log("SYSTEM SETUP");
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab, this.transform);

        foreach(var unit in UnitManager.Instance.GetUnitList())
        {
            var gridPos = unit.GetGridPosition();
            GetNode(gridPos.x, gridPos.z).SetIsWalkable(false);
        }

        //GridPosition gridPosition = new GridPosition(x, z);


        /* for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 10f;
                // if(Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, nonWalkableLayer)){
                //     GetNode(x, z).SetIsWalkable(false);
                // }
                // if(Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, nonWalkableLayer) && gridObject.HasAnyUnit()){
                // GetNode(x, z).SetIsWalkable(false);
                // }
            }
        }
        */


    }

    public void UpdateNode(UnitBase unit, GridPosition newPos)
    {
        GetNode(unit.GridPosition.x, unit.GridPosition.z).SetIsWalkable(true);
        GetNode(newPos.x, newPos.z).SetIsWalkable(false);
    }

    public void OnUnitDestroy(UnitBase unit)
    {
        GetNode(unit.GridPosition.x, unit.GridPosition.z).SetIsWalkable(true);
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        List<PathNode> openList = new List<PathNode>(); //openlist contain all the nodes that are qued for searching /jp:openlist には，検索対象となるすべてのノードが含まれます．
        List<PathNode> closedList = new List<PathNode>(); //closedList contain all the nodes that we have already searched /jp:closedList には，既に検索した全てのノードが含まれる．

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        //Initialize all the nodes for the pathfinding
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        /*start searching for a path*/

        //calculation for a node
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        //open a cycle where the algorithm will keep runing the open list has some elements to search
        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                //reached the final node
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //cycle through the list of neighbours
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }
                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(currentNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        //not path found
        return null;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int distance = Mathf.Abs(gridPositionDistance.x) + Mathf.Abs(gridPositionDistance.z);
        return distance * MOVE_STRAIGHT_COST;

        //in case of diagonal move we will need x and z distance separatedly, also a ramining (xdis-zdis)
        //and finally return diagonalcost+straight cost*remaining
        /*int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;*/


    }

    //grab the node from the list that has the lower cost value in orther to help the algorith to finish quicker
    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }
    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));

    }
    //search for the neighbours of the node
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        GridPosition gridPosition = currentNode.GetGridPosition();
        /*neighbours to the node*/
        if (gridPosition.x - 1 >= 0)
        {
            //left node
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
        }
        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //right node
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
        }
        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            //upper node
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }
        if (gridPosition.z - 1 >= 0)
        {
            //down node
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }

        return neighbourList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;

    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        if (gridSystem == null) Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        Debug.Log("GridSys: " + gridSystem + " / gridPos: " + gridPosition);
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition){
        return FindPath(startGridPosition, endGridPosition)!= null;
    }

}
