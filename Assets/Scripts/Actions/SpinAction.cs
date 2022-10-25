using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
 
    private float totalSpinAmount;
    

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }
        float spinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);

        totalSpinAmount += spinAmount;
        if (totalSpinAmount>= 360f)
        {
            ActionComplete();

        }
      
    }

    #region Switch Method
    /*
    public override void Spin(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        this.targetposition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }
    */
    #endregion

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
  
        totalSpinAmount = 0f;

    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition>
        {
            unitGridPosition,
        };
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

}
