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

    private Vector3 targetposition;
 

    protected override void Awake()
    {
        base.Awake();
        targetposition = transform.position;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!isActive)
        {
            return;
        }
        Vector3 moveDirection = (targetposition - transform.position).normalized;
        
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetposition) > stoppingDistance)
        {

            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            
        }
        else
        {
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();

        }
        float rotateSpeed = 20f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed); 
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
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        
        this.targetposition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
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

                validGridPositionsList.Add(testGridPosition);
                //Debug.Log(testGridPosition);
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
        int targetCountAtGridPosition = unit.GetAttackAction().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10
        };
    }
}
