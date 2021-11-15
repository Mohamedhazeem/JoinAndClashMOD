using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;
    [Header("Target")]
    public Transform targetTransform;
    public bool isTargetAvailable;
    [Header("Player Component")]
    public Player player;
    [Header("Health")]
    public float health;
    [Header("Attack Power")]
    public float attackPower;

    public void Start()
    {
        isTargetAvailable = true;
    }
    public void Update()
    {
        if (isTargetAvailable)
        {
            Attack(targetTransform);
        }
    }
    public void Attack(Transform target)
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) < 2)
            {
                AttackAnimation();
            }
        }
        else
        {
            var newTarget = PlayerManager.instance.PlayerAndNPCTransform();

            if (newTarget != null)
            {
                targetTransform = newTarget;
                player = targetTransform.GetComponent<Player>();
            }
            else
            {
                EnemyManager.instance.currentEnemyStates = EnemyStates.Idle;
                Idle();
                isTargetAvailable = false;
            }
        }
    }
    public void Attack()
    {
        if (targetTransform != null)
        {
            player.health -= attackPower;
            if (player.health <= 0)
            {
                player.CheckHealth();
                targetTransform = null;
                player = null;
            }
        }
    }
    public void Idle()
    {
        if (EnemyManager.instance.currentEnemyStates == EnemyStates.Idle)
        {
            animator.SetBool(Animator.StringToHash("Idle"), true);
        }
    }
    public void AttackAnimation()
    {
        animator.SetBool(Animator.StringToHash("Attack"), true);
    }
}

