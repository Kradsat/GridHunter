using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : EnemyAction
{
    private enum TURN_ACTION
    {
        Attack = 0,
        Predict,
        AOE,
        MAX,
    }

    private enum AOE_TYPE
    {
        SQUARE = 0,
        CROSS,
        MAX,
    }

    private int _turn = 0;
    private int _aoe_type = (int)AOE_TYPE.MAX;

    public void Attack()
    {
        switch (_turn % (int)TURN_ACTION.MAX)
        {
            case (int)TURN_ACTION.Attack:
                NormalAttack();
                break;
            case (int)TURN_ACTION.Predict:
                NormalAttack();
                PickAoeType();
                ShowPrediction();
                break;
            case (int)TURN_ACTION.AOE:
                HidePrediction();
                AreaAttack();
                break;
        }
    }

    private void NormalAttack()
    {
        //attack
    }

    private void PickAoeType()
    {
        _aoe_type = Random.Range(0, (int)AOE_TYPE.MAX);
    }

    private void ShowPrediction()
    {
        // show ui
    }

    private void HidePrediction()
    {
        //hide ui
    }

    private void AreaAttack()
    {
        //aoe
    }
}
