using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Boss2Action : EnemyAction
{
    //　アクション
    private enum TURN_ACTION
    {
        Attack = 0,
        Range,
        AOE,
        Predict,
        MAX,
    }

    [Header("ターンの行動")]
    [SerializeField] private List<TURN_ACTION> _turn_action = new List<TURN_ACTION>();

    [Header("Normal Attack Damage")]
    [SerializeField] private int _normal_damage = 280;

    [Header("Range Damage")]
    [SerializeField] private int _range_damage = 280;

    [Header("AOE Damage")]
    [SerializeField] private int _aoe_damage = 3000;

    private int _now_damage;

    private bool _isPredictionOn = false;

    //　ボスサイズ
    private const int BOSS_SIZE = 4;
    // 四角範囲攻撃のサイズ
    private const int SQUARE_WIDTH = 1;
    private const int RANGE_SIZE = 5;

    // ターン数
    private int _turn = 0;

    // 四角攻撃のエリア
    private static List<GridPosition> _square_area_list = new List<GridPosition>();

    // 遠隔攻撃のエリア
    private static List<GridPosition> _range_area_list = new List<GridPosition>();

    // AOEのエリア
    private static List<GridPosition> _aoe_range_list = new List<GridPosition>();

    // 現在の攻撃エリア
    private List<GridPosition> _now_attack_area = new List<GridPosition>();

    // AOE無効範囲
    private List<GridPosition> _no_damage_area = new List<GridPosition>()
    {
        new GridPosition(12, 14),
        new GridPosition(1, 12),
        new GridPosition(6, 6),
        new GridPosition(11, 9),new GridPosition(12, 9),new GridPosition(13, 9),
        new GridPosition(2, 5),new GridPosition(3, 5),new GridPosition(4, 5),
        new GridPosition(10, 3),
        new GridPosition(5, 2)
    };

    // 初期化
    public override void Init(UnitStruct unit)
    {
        base.Init(unit);

        var xPos = base.GridPosition.x - 1;
        var zPos = base.GridPosition.z - 1;
        SetNormalArea(xPos, zPos);
        SetAoeArea();
    }

    public override void Update()
    {
        base.Update();

        // ターンが予兆の際、予測エリアを表示する
        if (_turn_action[_turn % (int)_turn_action.Count] == TURN_ACTION.Predict && _isPredictionOn)
        {
            GridSystemVisual.Instance.ShowPrediction(_now_attack_area);
        }
    }

    // 攻撃する際に呼ぶ
    public override void Attack(Action callback = null)
    {
        _turn = TurnSystem.Instance.TurnNumber / 2 - 1;
        var action = _turn % (int)_turn_action.Count;
        _isPredictionOn = false;

        if (_turn_action[action] == TURN_ACTION.Predict)
        {
            CheckAction(action, callback);
            _isPredictionOn = true;
        }
        else if (_turn_action[action] == TURN_ACTION.Attack)
        {
            _now_attack_area = _square_area_list;
            _now_damage = _normal_damage;
            AreaAttack(callback);
        } else
        {
            AreaAttack(callback);
        }
    }

    /// <summary>
    /// 四角攻撃する際の範囲セット
    /// </summary>
    private void SetNormalArea(int x, int z)
    {
        _square_area_list.Clear();
        for (var zPos = x - SQUARE_WIDTH; zPos < x + SQUARE_WIDTH + BOSS_SIZE; zPos++)
        {
            for (var xPos = z - SQUARE_WIDTH; xPos < z + SQUARE_WIDTH + BOSS_SIZE; xPos++)
            {
                _square_area_list.Add(new GridPosition(zPos, xPos));
            }
        }
    }

    /// <summary>
    /// 遠隔範囲セット
    /// </summary>
    private void SetRangeArea(int x, int z)
    {
        _range_area_list.Clear();
        for (var zPos = x - RANGE_SIZE / 2; zPos <= x + RANGE_SIZE / 2; zPos++)
        {
            for (var xPos = z - RANGE_SIZE / 2; xPos <= z + RANGE_SIZE / 2; xPos++)
            {
                _range_area_list.Add(new GridPosition(zPos, xPos));
            }
        }
    }

    /// <summary>
    /// AOEの範囲セット
    /// </summary>
    private void SetAoeArea()
    {
        for (var x = 0; x < 10; x++)
        {
            for (var z = 0; z < 10; z++)
            {
                _aoe_range_list.Add(new GridPosition(x, z));
            }
        }

        // remove no damage area
        _no_damage_area.ForEach(_ => {
            if (_aoe_range_list.Contains(_))
            {
                _aoe_range_list.Remove(_);
            }
        });
    }

    /// <summary>
    /// 予兆範囲を更新
    /// </summary>
    private void CheckAction(int nowActionCount, Action callback)
    {
        var nextAction = nowActionCount + 1;

        if (_turn_action[nextAction] == TURN_ACTION.Range)
        {
            var target = base.UpdateAttackTarget();
            SetRangeArea(target.GetGridPosition().x, target.GetGridPosition().z);
            _now_attack_area = _range_area_list;
            _now_damage = _range_damage;
        }
        else if (_turn_action[nextAction] == TURN_ACTION.AOE)
        {
            _now_attack_area = _aoe_range_list;
            _now_damage = _aoe_damage;
        }

        callback();
    }

    /// <summary>
    /// 範囲攻撃
    /// </summary>
    private void AreaAttack(Action callback)
    {
        foreach (var targetGrid in _now_attack_area)
        {
            if (UnitManager.Instance.GetAllyUnitList().Any(_ => _.GridPosition == targetGrid))
            {
                UnitManager.Instance.GetAllyUnitList().Find(_ => _.GridPosition == targetGrid)?.Damage(_now_damage);
                UnitManager.Instance.GetEnemyActionList().Find(_ => _.GridPosition == targetGrid && _.Unit.Id != (int)MapData.OBJ_TYPE.BOSS2 && _.Unit.Id != (int)MapData.OBJ_TYPE.NEST)?.Damage(_now_damage);
            }
        }
        callback();
    }
}
