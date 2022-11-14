using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    //[SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;


    private List<Vector3> positionList;
    private int currentPositionIndex;

    private Quaternion originalRotationValue;
    float rotationResetSpeed = 20f;


    protected override void Awake()
    {
        base.Awake();
        //positionList = transform.position;
    }

    void Start()
    {
        originalRotationValue = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isActive)
        {
            return;
        }
        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float rotateSpeed = 20f;
        transform.forward = Vector3.Lerp( transform.forward, moveDirection, Time.deltaTime * rotateSpeed );

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {

            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;



        }
        else
        {
            currentPositionIndex ++;
            if(currentPositionIndex>=positionList.Count){
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                unit.canMove = false;
                UnitBase selectedUnit = UnitActionSystem.Instance.GetSelectedUnit( );
                if ( !unit.IsEnemy )
                foreach( BaseAction baseAction in selectedUnit.GetBaseActionArray( ) ) {
                    if( baseAction.GetActionName( ) == "Attack" ) {
                        UnitActionSystem.Instance.SetSelectedAction( baseAction );
                        break;
                    }
                }
            }

            //transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationValue, Time.deltaTime * rotationResetSpeed);


        }


    }

    #region Switch Method
    /*
    public override void Move(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        this.targetposition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }
    */
    #endregion


    #region Generic Method
    public override void TakeAction(GridPosition targetGridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), targetGridPosition);
       currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        Pathfinding.Instance.UpdateNode(unit, pathGridPositionList[pathGridPositionList.Count - 1]);

        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }
    #endregion

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionsList = new List<GridPosition>();
        GridPosition unitGridPostion = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPostion + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPostion==testGridPosition)
                {
                    //same grid position where the unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //grid position already ocupied with other unit
                    continue;
                }
                //if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                //{
                //    continue;
                //}
                //if (!Pathfinding.Instance.HasPath(unitGridPostion, testGridPosition))
                //{
                //    continue;
                //}

                validGridPositionsList.Add(testGridPosition);
            }
        }

        return validGridPositionsList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<AttackAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10
        };
    }

    public override int GetActionPointsCost()
    {
        if (unit.canMove && unit.canStay)
        {
            return 1;
        }
        return 2;
    }

}
