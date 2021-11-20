using System;
using UnityEngine;
public class CastleEnemy : Enemy
{
    [Header("Target")]
    public Transform targetTransform;
    public bool isTargetAvailable;
    [Header("Player Component")]
    public Player player;
    [Header("Health")]
    public float health;
    [Header("Attack Power")]
    public float attackPower;
    protected override void Start()
    {
        base.Start();
        isTargetAvailable = true;
        PlayerManager.instance.OnClimaxWinAnimation += Win;
    }

    private void Win()
    {
        if (EnemyManager.instance.currentEnemyStates == EnemyStates.Win)
        {
            animator.SetBool(Animator.StringToHash("Attack"), false);
            animator.SetTrigger(Animator.StringToHash("Win"));
        }
    }
    protected override void Update()
    {
        
        if (isTargetAvailable && EnemyManager.instance.currentEnemyStates == EnemyStates.Chase)
        {
            Chase(targetTransform);           
        }
    }

    public override void Chase(Transform target)
    {
        if (target != null)
        {
            if(Vector3.Distance(transform.position,target.position) > 2)
            {
                base.Chase(target);             
            }
            else
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

                player.castleEnemy = this;
                player.targetTransform = this.transform;
                player.isTargetAvailable = true;
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
    public void AttackAnimation()
    {      
            animator.SetBool(Animator.StringToHash("Attack"), true);                
    }
    public void Attack()
    {
        if(targetTransform != null)
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

    internal void CheckHealth()
    {
        if (health <= 0)
        {
            Die();
        }
    }
}
