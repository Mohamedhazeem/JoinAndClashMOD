using System;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Capsule collider")]
    public CapsuleCollider capsuleCollider;
    public float capsuleColliderHeight;

    [Header("Target")]
    public Transform targetTransform;
    public bool isTargetAvailable = false;

    [Header("Player Component")]
    public Player player;

    [Header("Health")]
    public float health;

    [Header("Attack Power")]
    public float attackPower;

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
            if (Vector3.Distance(transform.position, target.position) < 6)
            {
                var pos = target.position;
                pos.y = 0.25f;
                target.position = pos;
                transform.LookAt(target);
                EnemyManager.instance.currentEnemyStates = EnemyStates.Attack;
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
                EnemyManager.instance.currentEnemyStates = EnemyStates.Win;
                GameManager.instance.currentGameState = GameManager.GameState.Lose;
                EnemyManager.instance.SwitchEnemyStates();
                GameManager.instance.SwitchGameStates();         
                isTargetAvailable = false;
            }
        }
    }
    public void AttackPlayer()
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
            animator.SetBool(Animator.StringToHash("Attack"), false);
            animator.SetBool(Animator.StringToHash("Idle"), true);
        }
    }
    public void AttackAnimation()
    {
        if (EnemyManager.instance.currentEnemyStates == EnemyStates.Attack)
        {
            animator.SetBool(Animator.StringToHash("Idle"), false);
            animator.SetBool(Animator.StringToHash("Attack"), true);
        }
    }
    public void Win()
    {
        if (EnemyManager.instance.currentEnemyStates == EnemyStates.Win)
        {
            animator.SetBool(Animator.StringToHash("Attack"), false);
            animator.SetTrigger(Animator.StringToHash("Win"));
        }        
    }

    internal void CheckHealth()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger(Animator.StringToHash("Die"));
        capsuleCollider.height = capsuleColliderHeight;
        EnemyManager.instance.enemyList.Remove(this.gameObject);
        Destroy(this.gameObject, 0.5f);
    }
}

