using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAction : BaseAction
{
    private float totalSpinAmount;


    // Update is called once per frame
    void Update( ) {
        if( !isActive ) {
            return;
        }

        ActionComplete( );

    }

    public override void TakeAction( GridPosition gridPosition, Action onActionComplete ) {
        ActionStart( onActionComplete );

    }

    public override string GetActionName( ) {
        return "Stay";
    }

    public override List<GridPosition> GetValidActionGridPositionList( ) {
        GridPosition unitGridPosition = unit.GetGridPosition( );
        return new List<GridPosition>
        {
            unitGridPosition,
        };
    }

    public override EnemyAIAction GetEnemyAIAction( GridPosition gridPosition ) {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
