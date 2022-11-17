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
        foreach (var enemy in UnitManager.Instance.GetEnemyActionList())
        {
            enemy.Attack(() => {
                Debug.Log("CALLBACK BY " + enemy.Unit.Id);
                _enemiesActionCount++;
                if (_enemiesActionCount >= (UnitManager.Instance.GetEnemyActionList()).Count)
                {
                    TurnSystem.Instance.NextTurn();
                }
            });
        }
    }
}


