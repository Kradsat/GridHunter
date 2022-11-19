using System;
using UnityEngine;
using System.Collections;

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
    private bool _isAttacking = false;

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
                    //TryTakeEnemiesAIAction();
                }
                break;
            case State.Busy:
                StartCoroutine(TakeEnemyAction(_enemiesActionCount));
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

    private void TryTakeEnemiesAIAction()
    {
        foreach (var enemy in UnitManager.Instance.GetEnemyActionList())
        {
            _isAttacking = true;
            enemy.Attack(() => {
                _isAttacking = false;
                _enemiesActionCount++;
                if (_enemiesActionCount >= (UnitManager.Instance.GetEnemyActionList()).Count)
                {
                    TurnSystem.Instance.NextTurn();
                }
            });
        }
    }

    private IEnumerator TakeEnemyAction(int index)
    {
        if (_isAttacking)
        {
            yield break;
        }

        Debug.Log("ATTACK ENTER");
        _isAttacking = true;

        UnitManager.Instance.GetEnemyActionList()[index].Attack(() => {
            Debug.Log("Attack Callback");
            _enemiesActionCount++;
            _isAttacking = false;
            if (_enemiesActionCount >= (UnitManager.Instance.GetEnemyActionList()).Count)
            {
                _enemiesActionCount = 0;
                TurnSystem.Instance.NextTurn();
            }
        });
    }
}


