using UnityEngine;
using System;

public class UnitBase : UnitStatus
{
    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawn;
    public static event EventHandler OnAnyUnitDead;
    public static event EventHandler OnHPChange;

    [SerializeField] public bool canMove = true;
    [SerializeField] public bool canAttack = true;
    [SerializeField] public bool canStay = true;

    private GridPosition gridPosition;
    public GridPosition GridPosition { get { return gridPosition; } }
    private BaseAction[] baseActionArray;
    private int actionPoints = ACTION_POINTS_MAX;

    private void Awake()
    {
        baseActionArray = GetComponents<BaseAction>();
    }

    public override void Init(UnitStruct unit)
    {
        base.Init(unit);

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        OnAnyUnitSpawn?.Invoke(this, EventArgs.Empty);
    }

    public virtual void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            //unit has changed position
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }


    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else { return false; }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy && !TurnSystem.Instance.IsPlayerTurn) ||
            (!IsEnemy && TurnSystem.Instance.IsPlayerTurn))
        {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            canMove = true;
            canAttack = true;
            canStay = true;
        }
    }

    public void Damage(int damageAmount, Action callback = null)
    {
        base.HP -= (double)damageAmount;
        if( base.HP < 0 ) {
            base.HP = 0;
        }
        callback?.Invoke();
        OnHPChange?.Invoke(this, EventArgs.Empty);

        if (base.HP <= 0)
        {
            LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
            Destroy(gameObject);
            TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
            OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
        }
    }

    public void HealPlayer(int healAmount)
    {
        base.HP += ( double )healAmount;
        if (base.HP > base.MAX_HP)
        {
            base.HP = base.MAX_HP;
            OnHPChange?.Invoke(this, EventArgs.Empty);
        } else {
            OnHPChange?.Invoke( this, EventArgs.Empty );
        }
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

}
