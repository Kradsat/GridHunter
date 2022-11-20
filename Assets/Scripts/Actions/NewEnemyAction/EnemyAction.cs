using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class EnemyAction : UnitBase
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    public event EventHandler OnAttackStart;

    [Header("Movement")]
    [SerializeField] private float _rotateSpeed = 20f;
    [SerializeField] private float _stoppingDistance = .1f;
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private int _moveDistance = 4;

    [Header("Attack Mode")]
    [SerializeField] private EnemyActionBase.AttackMode _attackMode;
    [SerializeField] private List<EnemyActionBase.TargetJob> _targetJob = new List<EnemyActionBase.TargetJob> {
        EnemyActionBase.TargetJob.ROD , EnemyActionBase.TargetJob.SWORD, EnemyActionBase.TargetJob.HAMMER, EnemyActionBase.TargetJob.LANCE
    };
    private int _targetIndex = 0;

    [Header("Attack Distance")]
    [SerializeField] private bool _isShowAttackDistance = true;
    [SerializeField] private int _attackDistance = 1;
    private static List<GridPosition> _attackDistance1 = new List<GridPosition>()
    {
        new GridPosition(-1,0),new GridPosition(1,0),new GridPosition(0,1),new GridPosition(0,-1),
    };
    private static List<GridPosition> _attackDistance2 = new List<GridPosition>()
    {
        new GridPosition(-2,0),new GridPosition(2,0),new GridPosition(0,2),new GridPosition(0,-2),
        new GridPosition(1,-1),new GridPosition(1,0),new GridPosition(1,1),
        new GridPosition(0,-1),new GridPosition(0,1),
        new GridPosition(-1,-1),new GridPosition(-1,0),new GridPosition(-1,1),
    };
    private List<List<GridPosition>> _attackDistanceList = new List<List<GridPosition>>
    {
        _attackDistance1, _attackDistance2
    };

    private Action _attackCallback = null;
    
    private int currentPositionIndex = 0;
    private List<Vector3> positionList;
    private bool isActive = false;

    private UnitBase _target = null;
    private List<UnitBase> _playerUnitList;
    private List<GridPosition> pathGridPositionList = null;

    public override void Update()
    {
        base.Update();

        if (!isActive)
        {
            return;
        }

        if (this.Unit.Id == (int)MapData.OBJ_TYPE.BOSS)
        {
            AttackTarget();
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];

        if (Vector3.Distance(transform.position, targetPosition) > _stoppingDistance)
        {
            //var dir = (this.transform.position - targetPosition).normalized;
            //Debug.Log(dir);
            //this.transform.rotation = Quaternion.Euler(0, 90 * dir.x, 0);

            Vector3 relativePos = transform.position - targetPosition;
            this.transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);

            var step = _moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            Pathfinding.Instance.UpdateNode(this, pathGridPositionList[currentPositionIndex]);
        }
        else
        {
            if(currentPositionIndex < _moveDistance)
            {
                currentPositionIndex++;
                if (currentPositionIndex >= positionList.Count)
                {
                    isActive = false;
                    OnStopMoving?.Invoke(this, EventArgs.Empty);
                    this.canMove = false;
                    Vector3 relativePos = transform.position - _target.transform.position;
                    this.transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    AttackTarget();
                }
            }
            else
            {
                isActive = false;
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                this.canMove = false;
                _attackCallback?.Invoke();
            }

        }
    }

    public virtual void Attack(Action callback = null)
    {
        _attackCallback = callback;
        GetAttackTarget();
        if (_playerUnitList.Count == 0)
        {
            callback?.Invoke();
            return;
        }
        FindPathToTarget();
    }

    public void GetAttackTarget()
    {
        _playerUnitList = UnitManager.Instance.GetAllyUnitList();

        switch (_attackMode)
        {
            case EnemyActionBase.AttackMode.HP_HIGH:
                _target = GetHighestHPUnit();
                break;
            case EnemyActionBase.AttackMode.HP_LOW:
                _target = GetLowestHPUnit();
                break;
            case EnemyActionBase.AttackMode.Distance:
                _target = GetClosestUnit();
                break;
            case EnemyActionBase.AttackMode.Job:
                _target = GetSpecificJobUnit();
                break;
        }
    }

    private UnitBase GetHighestHPUnit()
    {
        if (_playerUnitList.Count <= 0)
        {
            return null;
        }

        var _target = _playerUnitList[0];
        double _highestHP = 0;

        foreach (var unit in _playerUnitList)
        {
            var HP = unit.HP;
            if (_highestHP < HP)
            {
                _highestHP = HP;
                _target = unit;
            }
        }

        return _target;
    }

    private UnitBase GetLowestHPUnit()
    {
        if (_playerUnitList.Count <= 0)
        {
            return null;
        }

        var target = _playerUnitList[0];
        double _lowestHP = 1000;

        foreach (var unit in _playerUnitList)
        {
            var HP = unit.HP;
            if(_lowestHP > HP)
            {
                _lowestHP = HP;
                target = unit;
            }
        }

        return target;
    }

    private UnitBase GetClosestUnit()
    {
        if (_playerUnitList.Count <= 0)
        {
            return null;
        }

        var target = _playerUnitList[0];
        float _shortestDistance = 1000;
        var _selfVec2 = new Vector2(base.GridPosition.x, base.GridPosition.z);

        foreach (var unit in _playerUnitList)
        {
            var targetVec2 = new Vector2(unit.GridPosition.x, unit.GridPosition.z);
            var distance = Vector2.Distance(_selfVec2, targetVec2);
            if (_shortestDistance > distance)
            {
                _shortestDistance = distance;
                target = unit;
            }
        }
        return target;
    }

    public UnitBase GetSpecificJobUnit()
    {
        var target = _playerUnitList[0];

        if (_playerUnitList.Count != _targetJob.Count)
        {
            _targetIndex = (_targetJob.Count - _playerUnitList.Count);
            Debug.Log("TARGET INDEX: " + _targetIndex);
        }

        if(_targetJob.Count <= 0)
        {
            return null;
        }

        target = _playerUnitList.FirstOrDefault(_ => _.Unit.Id == (int)_targetJob[_targetIndex]);

        return target;
    }



    public void FindPathToTarget()
    {
        if(_target == null)
        {
            _playerUnitList = UnitManager.Instance.GetAllyUnitList();
            _target = _playerUnitList[0];
        }

        var targetGridPosition = _target.GridPosition;

        if(this.Unit.Id == (int)MapData.OBJ_TYPE.BOSS)
        {
            isActive = true;
            return;
        }

        var unitPos = base.GridPosition;
        var targetNeighbours = new List<GridPosition>();
        var targetNeighbourPaths = new List<List<GridPosition>>();
        foreach(var offset in _attackDistanceList[_attackDistance - 1])
        {
            var neighbour = targetGridPosition + offset;
            if (GridPosition.CheckIfInside(neighbour) && neighbour != targetGridPosition && !LevelGrid.Instance.HasAnyUnitOnGridPosition(neighbour))
            {
                if (Pathfinding.Instance.FindPath(GridPosition, neighbour) != null)
                {
                    targetNeighbours.Add(neighbour);
                    targetNeighbourPaths.Add(Pathfinding.Instance.FindPath(GridPosition, neighbour));
                }
            }
        }

        if (_isShowAttackDistance)
        {
            GridSystemVisual.Instance.ShowAoePrediction(targetNeighbours);
        }

        pathGridPositionList = null;
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
        isActive = false;
        Debug.Log(this + " / Attack Target: " + _target);
        _target.Damage(this.Unit.Attack);
        _attackCallback?.Invoke();
    }

    private void OnDestroy()
    {
        UnitManager.Instance.RemoveEnemyAction(this);
    }
}
