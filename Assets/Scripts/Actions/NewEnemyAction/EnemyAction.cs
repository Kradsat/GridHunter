using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class EnemyAction : UnitBase
{
    [SerializeField] private EnemyActionBase.AttackMode attackMode;
    [SerializeField] private EnemyActionBase.TargetJob targetJob;

    private List<UnitBase> _playerUnitList;
    public List<UnitBase> SetPlayerUnitList { set { _playerUnitList = value; } }

    public UnitBase GetAttackTarget()
    {
        UnitBase _targetUnit = null;

        switch (attackMode)
        {
            case EnemyActionBase.AttackMode.HP:
                _targetUnit = GetLowestHPUnit();
                break;
            case EnemyActionBase.AttackMode.Distance:
                _targetUnit = GetClosestUnit();
                break;
            case EnemyActionBase.AttackMode.Job:
                _targetUnit = GetSpecificJobUnit();
                break;
        }

        return _targetUnit;
    }

    private UnitBase GetLowestHPUnit()
    {
        var _target = _playerUnitList[0];
        var _lowestHP = 1000;

        foreach (var unit in _playerUnitList)
        {
            var HP = unit.HP;
            if(HP < _lowestHP)
            {
                _target = unit;
            }
        }

        return _target;
    }

    private UnitBase GetClosestUnit()
    {
        var _target = _playerUnitList[0];
        var _shortestDistance = 100;

        foreach (var unit in _playerUnitList)
        {
            var distance = (unit.GridPosition.z - base.GridPosition.z) / (unit.GridPosition.x - base.GridPosition.x);
            if (distance < _shortestDistance)
            {
                _shortestDistance = distance;
                _target = unit;
            }
        }

        return _target;
    }

    public UnitBase GetSpecificJobUnit()
    {
        var _playerUnitList = UnitManager.Instance.GetAllyUnitList();
        var _target = _playerUnitList.FirstOrDefault(_ => _.Unit.Id == (int)targetJob);

        return _target;
    }
}
