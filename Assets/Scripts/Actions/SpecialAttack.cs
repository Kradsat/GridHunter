using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : BaseAction
{
    public static event EventHandler OnAnySwordHit;

    public event EventHandler OnAttackStart;
    public event EventHandler OnAttackEnd;


    [SerializeField] private int maxAttackDistance = 5;
    [SerializeField] private int healAmount = 50;
    [SerializeField] AudioSource audioSource;

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,

    }

    private State state;
    private float stateTimer;
    private bool canUseSpecialAttack;
    private UnitBase targetUnit;



    // Update is called once per frame
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        #region 3 states
        /*
        switch (state)
        {
            case State.Aiming:
                Vector3 aimDir = targetUnit.GetWorldPosition()-unit.GetWorldPosition();

                float rotateSpeed = 5f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.Attacking:
                if (canAttack)
                {
                    Attack();
                    canAttack = false;
                }
                break;
            case State.Cooloff:
                break;
        }*/
        #endregion

        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                Vector3 aimDir = targetUnit.GetWorldPosition() - unit.GetWorldPosition();

                float rotateSpeed = 5f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.SwingingSwordAfterHit:
                break;
            default:
                break;
        }
        //Debug.Log(state);
        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void Attack()
    {
        //OnAttackStart?.Invoke(this, EventArgs.Empty);
        //targetUnit.Damage();
    }

    private void NextState()
    {
        #region 3 states
        /*
        switch (state)
        {
            case State.Aiming:
                state = State.Attacking;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Attacking:
                state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;

        }
        */
        #endregion

        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                targetUnit.HealPlayer(healAmount);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnAttackEnd?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                //audioSource.Stop();
                unit.canAttack = false;
                break;

        }
        //Debug.Log(state);
    }

    public override string GetActionName()
    {
        return "Heal Ally";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionsList = new List<GridPosition>();


        for (int x = -maxAttackDistance; x <= maxAttackDistance; x++)
        {
            for (int z = -maxAttackDistance; z <= maxAttackDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                //validGridPositionsList.Add(testGridPosition);

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
                    continue;
                }

                UnitBase targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (!targetUnit.IsEnemy == unit.IsEnemy)
                {
                    // Both Units on same 'team'
                    continue;
                }

                validGridPositionsList.Add(testGridPosition);

                //Debug.Log(testGridPosition);
            }
        }

        return validGridPositionsList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {


        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        // Debug.Log("Aiming");
        state = State.SwingingSwordBeforeHit;
        float aimingStateTime = .7f;
        stateTimer = aimingStateTime;
        //audioSource.Play();
        OnAttackStart?.Invoke(this, EventArgs.Empty);
        //canAttack = true;

        ActionStart(onActionComplete);
    }

    public UnitBase GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxAttackDistance()
    {
        return maxAttackDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100,
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

    public override int GetActionPointsCost()
    {
        if (unit.canAttack && unit.canMove == false)
        {
            return 1;
        }
        return 2;
    }
}
