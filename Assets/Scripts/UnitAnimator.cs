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
            attackAction.OnAttackEnd += AttackAction_OnAttackEnd;
        }

        if (TryGetComponent<SpecialAttack>(out SpecialAttack specialAttack))
        {
            specialAttack.OnAttackStart += SpecialAttack_OnAttackStart;
            specialAttack.OnAttackEnd += SpecialAttack_OnAttackEnd;
        }
        //healthSystem = GetComponent<HealthSystem>();
        //healthSystem.OnDead += HealthSystem_OnDead;

    }

    private void SpecialAttack_OnAttackStart(object sender, EventArgs e)
    {
        animator.SetTrigger("SpecialAttack");
         var childComponents = GetComponentsInChildren<Animator>();
        foreach (var child in childComponents)
        {
           child.SetBool("Attack1",true);
        }
    }

    private void SpecialAttack_OnAttackEnd(object sender, EventArgs e)
    {
        animator.SetTrigger("SpecialAttack");
         var childComponents = GetComponentsInChildren<Animator>();
        foreach (var child in childComponents)
        {
           child.SetBool("Attack1",false);
        }
    }

    private void AttackAction_OnAttackStart(object sender, EventArgs e)
    {
        animator.SetBool("Attack1",true);
        var childComponents = GetComponentsInChildren<Animator>();
        foreach (var child in childComponents)
        {
           child.SetBool("Attack1",true);
        }
    }

     private void AttackAction_OnAttackEnd(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
        var childComponents = GetComponentsInChildren<Animator>();
        foreach (var child in childComponents)
        {
           child.SetBool("Attack1", false);
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
        var childComponents = GetComponentsInChildren<Animator>();
        foreach (var child in childComponents)
        {
           child.SetBool("IsWalking", true);
        }
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
        var childComponents = GetComponentsInChildren<Animator>();
        foreach (var child in childComponents)
        {
           child.SetBool("IsWalking", false);
        }
    }


}
