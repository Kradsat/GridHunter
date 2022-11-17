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

    private static List<GridPosition> _square_area_list = new List<GridPosition>()
    {

    };

    private static List<GridPosition> _cross_area_list = new List<GridPosition>()
    {

    };

    private List<List<GridPosition>> _area = new List<List<GridPosition>>
    {
        _square_area_list, _cross_area_list
    };

    private void Update()
    {
        if(_aoe_type != (int)AOE_TYPE.MAX)
        {
            GridSystemVisual.Instance.ShowAoePrediction(_area[_aoe_type]);
        }
    }

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
                break;
            case (int)TURN_ACTION.AOE:
                AreaAttack();
                break;
        }
    }

    private void NormalAttack()
    {
        base.GetAttackTarget();
    }

    private void PickAoeType()
    {
        _aoe_type = Random.Range(0, (int)AOE_TYPE.MAX);
    } 

    private void AreaAttack()
    {
        _aoe_type = (int)AOE_TYPE.MAX;
    }
}
