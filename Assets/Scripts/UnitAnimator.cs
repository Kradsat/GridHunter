using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    //[SerializeField] private Animator enemyAnimator;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();


        if (TryGetComponent<EnemyAction>(out EnemyAction enemyAction))
        {
            enemyAction.OnStartMoving += MoveAction_OnStartMoving;
            enemyAction.OnStopMoving += MoveAction_OnStopMoving;
            enemyAction.OnAttackStart += AttackAction_OnAttackStart;
        }
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<AttackAction>(out AttackAction attackAction))
        {
            attackAction.OnAttackStart += AttackAction_OnAttackStart;
            //attackAction.OnAttackEnd += AttackAction_OnAttackEnd;
        }

        if (TryGetComponent<SpecialAttack>(out SpecialAttack specialAttack))
        {
            specialAttack.OnAttackStart += SpecialAttack_OnAttackStart;
        }
        //healthSystem = GetComponent<HealthSystem>();
        //healthSystem.OnDead += HealthSystem_OnDead;

    }

    private void SpecialAttack_OnAttackStart(object sender, EventArgs e)
    {
        animator.SetTrigger("SpecialAttack");
    }

    private void AttackAction_OnAttackStart(object sender, EventArgs e)
    {
        animator.SetTrigger("Attack");
    }


    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }


}
