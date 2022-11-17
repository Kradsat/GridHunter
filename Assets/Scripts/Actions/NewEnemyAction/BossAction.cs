using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class BossAction : EnemyAction
{
    //　ターンに応じ、取るアクション
    private enum TURN_ACTION
    {
        Attack = 1,
        Predict,
        AOE,
        MAX,
    }

    // 範囲攻撃のタイプ
    private enum AOE_TYPE
    {
        SQUARE = 0,
        CROSS,
        MAX,
    }

    //　ボスサイズ
    private const int BOSS_SIZE = 4;
    // 四角範囲攻撃のサイズ
    private const int SQUARE_WIDTH = 1;

    private int _turn = 0;
    private int _aoe_type = (int)AOE_TYPE.MAX;

    // 四角攻撃のエリア
    private static List<GridPosition> _square_area_list = new List<GridPosition>();
    // 十字攻撃のエリア
    private static List<GridPosition> _cross_area_list = new List<GridPosition>();

    // 範囲攻撃エリアリスト
    private List<List<GridPosition>> _area = new List<List<GridPosition>>
    {
        _square_area_list, _cross_area_list
    };

    // 初期化
    public override void Init(UnitStruct unit)
    {
        base.Init(unit);
        var xPos = base.GridPosition.x - 1;
        var zPos = base.GridPosition.z - 1;

        SetSquareArea(xPos, zPos);
        SetCrossArea(xPos, zPos);
    }

    public override void Update()
    {
        base.Update();

        // AOEタイプがセットした際、予測エリアを表示する
        if(_aoe_type != (int)AOE_TYPE.MAX)
        {
            GridSystemVisual.Instance.ShowAoePrediction(_area[_aoe_type]);
        }
    }

    // 攻撃する際に呼ぶ
    public override void Attack(Action callback = null)
    {
        _turn = TurnSystem.Instance.TurnNumber / 2;
        switch (_turn % (int)TURN_ACTION.MAX)
        {
            case (int)TURN_ACTION.Attack:
                NormalAttack(callback);
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

    /// <summary>
    /// 普通攻撃
    /// </summary>
    private void NormalAttack(Action callback = null)
    {
        Debug.Log("NORMAL ATTACK");
        base.Attack(callback);
    }

    /// <summary>
    /// 範囲攻撃タイプをランダムで選択
    /// </summary>
    private void PickAoeType()
    {
        _aoe_type = UnityEngine.Random.Range(0, (int)AOE_TYPE.MAX);
    } 

    /// <summary>
    /// 範囲攻撃
    /// </summary>
    private void AreaAttack()
    {
        if (_aoe_type == (int)AOE_TYPE.MAX)
        {
            PickAoeType();
        }

        foreach(var gridPos in _area[_aoe_type])
        {
            //attack
        }
        _aoe_type = (int)AOE_TYPE.MAX;
    }

    /// <summary>
    /// 四角攻撃する際の範囲セット
    /// </summary>
    private void SetSquareArea(int x, int z)
    {
        for (var zPos = x - SQUARE_WIDTH; zPos < x + SQUARE_WIDTH + BOSS_SIZE; zPos++)
        {
            for (var xPos = z - SQUARE_WIDTH; xPos < z + SQUARE_WIDTH + BOSS_SIZE; xPos++)
            {
                _square_area_list.Add(new GridPosition(zPos, xPos));
            }
        }
    }

    /// <summary>
    /// 十字攻撃する際の範囲セット
    /// </summary>
    private void SetCrossArea(int x, int z){
        var height = LevelGrid.Instance.GetHeight();
        var width = LevelGrid.Instance.GetWidth();

        for (int zPos = z; zPos < z + BOSS_SIZE; zPos++)
        {
            for (int xPos = 0; xPos < width; xPos++)
            {
                _cross_area_list.Add(new GridPosition(xPos, zPos));
            }
        }

        for (int zPos = 0; zPos < height; zPos++)
        {
            for (int xPos = x; xPos < x +BOSS_SIZE; xPos++)
            {
                _cross_area_list.Add(new GridPosition(xPos, zPos));
            }
        }

        _cross_area_list.Distinct().ToList();
    }
}
