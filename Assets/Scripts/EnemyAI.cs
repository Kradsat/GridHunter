using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    private int _enemiesActionCount = 0;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn)
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    state = State.Busy;
                    _enemiesActionCount = 0;
                    TryTakeEnemiesAIAction(SetStateTakingTurn);
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn)
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    private void TryTakeEnemiesAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Take Enemy AI Action");
        foreach (var enemy in UnitManager.Instance.GetEnemyActionList())
        {
            Debug.Log("ENEMY ATTACK " + enemy.Unit.Id);
            enemy.Attack(() => {
            _enemiesActionCount++;
                if (_enemiesActionCount >= (UnitManager.Instance.GetEnemyActionList()).Count)
                {
                    TurnSystem.Instance.NextTurn();
                }
            });
        }
    }

    private bool TryTakeEnemyAIAction(EnemyAction enemy, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;
        foreach (BaseAction baseAction in enemy.GetBaseActionArray())
        {
            if (!enemy.CanSpendActionPointsToTakeAction(baseAction))
            {
                continue;
            }
            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }
        if (bestEnemyAIAction != null && enemy.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }

        //Debug.Log("???????????? " + enemy.Unit.Id);
        //BaseAction _baseAction = null;
        //UnitBase _target = enemy.GetAttackTarget();
        //foreach (BaseAction baseAction in enemy.GetBaseActionArray())
        //{
        //    Debug.Log("!!!!!!!!!!!!!! " + enemy.Unit.Id);
        //    if (!enemy.CanSpendActionPointsToTakeAction(baseAction))
        //    {
        //        //enemy cannot make the action
        //        continue;
        //    }
            
        //    _baseAction = baseAction;
        //}

        //Debug.Log("Base; " + _baseAction);
        //if (enemy.TrySpendActionPointsToTakeAction(_baseAction))
        //{
        //    _baseAction.TakeAction(_target.GetGridPosition(), onEnemyAIActionComplete);
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }
}


