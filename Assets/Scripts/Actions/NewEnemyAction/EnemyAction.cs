using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class EnemyAction : UnitBase
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    public event EventHandler OnAttackStart;

    [SerializeField] private EnemyActionBase.AttackMode attackMode;
    [SerializeField] private EnemyActionBase.TargetJob targetJob;

    private int currentPositionIndex = 0;
    private List<Vector3> positionList;
    private bool isActive = false;

    private UnitBase _target = null;

    private List<UnitBase> _playerUnitList;
    public List<UnitBase> SetPlayerUnitList { set { _playerUnitList = value; } }

    private Action _attackCallback = null;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (base.Unit.Id == (int)MapData.OBJ_TYPE.BOSS)
        {
            AttackTarget();
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float rotateSpeed = 20f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                isActive = false;
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                this.canMove = false;
                AttackTarget();
            }
        }
    }

    /// <summary>
    /// çUåÇ
    /// </summary>
    public virtual void Attack(Action callback = null)
    {
        _target = GetAttackTarget();
        FindPathToTarget();
        _attackCallback = callback;
    }

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
        return _target;
    }

    public UnitBase GetSpecificJobUnit()
    {
        var _playerUnitList = UnitManager.Instance.GetAllyUnitList();
        var _target = _playerUnitList.FirstOrDefault(_ => _.Unit.Id == (int)targetJob);

        return _target;
    }

    public void FindPathToTarget()
    {
        var targetGridPosition = _target.GridPosition;

        if(base.Unit.Id == (int)MapData.OBJ_TYPE.BOSS)
        {
            isActive = true;
            return;
        }

        var unitPos = base.GridPosition;
        var targetNeighbours = new List<GridPosition>();
        var targetNeighbourPaths = new List<List<GridPosition>>();
        for (int x = targetGridPosition.x - 1; x <= targetGridPosition.x + 1; x++)
        {
            for (int z = targetGridPosition.z - 1; z <= targetGridPosition.z + 1; z++)
            {
                var neighbour = new GridPosition(x, z);
                if(base.Unit.Id == 6)
                {
                    Debug.Log(neighbour);
                }
                if (GridPosition.CheckIfInside(neighbour) && neighbour != targetGridPosition && !LevelGrid.Instance.HasAnyUnitOnGridPosition(neighbour))
                {
                    if(Pathfinding.Instance.FindPath(GridPosition, neighbour) != null)
                    {
                        Debug.Log("ADDED NEIGHBOUR: " + neighbour + " / " + base.Unit.Id + " / " + _target);
                        Debug.Log("ADDED PATH");
                        targetNeighbours.Add(neighbour);
                        targetNeighbourPaths.Add(Pathfinding.Instance.FindPath(GridPosition, neighbour));
                    }
                }
            }
        }

        List<GridPosition> pathGridPositionList = null;
        int count = 100;
        foreach (var path in targetNeighbourPaths)
        {
            if (path.Count() < count)
            {
                count = path.Count();
                pathGridPositionList = path;
            }
        }

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        if (pathGridPositionList != null)
        {
            foreach (GridPosition pathGridPosition in pathGridPositionList)
            {
                positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
                Pathfinding.Instance.UpdateNode(this, pathGridPositionList[pathGridPositionList.Count - 1]);
                OnStartMoving?.Invoke(this, EventArgs.Empty);
                isActive = true;
            }
        }
        else
        {
            AttackTarget();
        }

    }

    private void AttackTarget()
    {
        Debug.Log("ATTACK");
        isActive = false;
        _target.Damage(this.Unit.Attack);
        _attackCallback?.Invoke();
    }
}
