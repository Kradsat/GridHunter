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
        unit.canStay = false;
        unit.canMove = false;
        UnitBase selectedUnit = UnitActionSystem.Instance.GetSelectedUnit( );
        foreach( BaseAction baseAction in selectedUnit.GetBaseActionArray( ) ) {
            if( baseAction.GetActionName( ) == "Attack" ) {
                UnitActionSystem.Instance.SetSelectedAction( baseAction );
                break;
            }
        }
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

    public override int GetActionPointsCost( ) {
        if( !unit.canStay ) {
            return 1;
        }
        return 2;
    }
}
