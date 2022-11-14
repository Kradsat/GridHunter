using UnityEngine;
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
        UnitBase _target = null;

        switch (attackMode)
        {
            case EnemyActionBase.AttackMode.HP:
                _target = GetLowestHPUnit();
                break;
            case EnemyActionBase.AttackMode.Distance:
                _target = GetClosestUnit();
                break;
            case EnemyActionBase.AttackMode.Job:
                _target = GetSpecificJobUnit();
                break;
        }

        Debug.Log(_target.Unit.Name);
        return _target;
    }

    private UnitBase GetLowestHPUnit()
    {
        var _target = _playerUnitList[0];
        double _lowestHP = 1000;

        foreach (var unit in _playerUnitList)
        {
            var HP = unit.HP;
            if(_lowestHP > HP)
            {
                _lowestHP = HP;
                _target = unit;
            }
        }

        Debug.Log(_target.Unit.Name);
        return _target;
    }

    private UnitBase GetClosestUnit()
    {
        var _target = _playerUnitList[0];
        float _shortestDistance = 1000;
        var _selfVec2 = new Vector2(base.GridPosition.x, base.GridPosition.z);

        foreach (var unit in _playerUnitList)
        {
            var targetVec2 = new Vector2(unit.GridPosition.x, unit.GridPosition.z);
            var distance = Vector2.Distance(_selfVec2, targetVec2);
            if (_shortestDistance > distance)
            {
                _shortestDistance = distance;
                _target = unit;
            }
        }

        Debug.Log(_target.Unit.Name);
        return _target;
    }

    public UnitBase GetSpecificJobUnit()
    {
        var _playerUnitList = UnitManager.Instance.GetAllyUnitList();
        var _target = _playerUnitList.FirstOrDefault(_ => _.Unit.Id == (int)targetJob);

        return _target;
    }
}
