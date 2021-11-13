using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Capsule collider")]
    public CapsuleCollider capsuleCollider;
    public float capsuleColliderHeight;

    [Header("Move Speed")]
    [SerializeField] private float moveSpeed;

    public bool isMove;
    private float timeToDisabled = 5f;

    protected virtual void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    protected virtual void Update()
    {
        if (isMove)
        {
            Move();
        }
        DiableEnemy();   
    }
    protected void StartRunAnimation()
    {
        if (EnemyManager.instance.currentEnemyStates == EnemyStates.Chase && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            animator.SetTrigger(Animator.StringToHash("Run"));
        }
        else if(EnemyManager.instance.currentEnemyStates == EnemyStates.Chase && GameManager.instance.currentGameState == GameManager.GameState.Climax)
        {
            animator.SetTrigger(Animator.StringToHash("Run"));
        }
    }
    public void Move()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        StartRunAnimation();
    }
    public virtual void Chase(Transform target)
    {
        if (EnemyManager.instance.currentEnemyStates == EnemyStates.Chase && (GameManager.instance.currentGameState == GameManager.GameState.GamePlay || GameManager.instance.currentGameState == GameManager.GameState.Climax))
        {
            var pos = target.position;
            pos.y = 0.25f;
            target.position = pos;
            transform.LookAt(target);            
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
            StartRunAnimation();
        }
    }
    public void Idle(Transform target)
    {
        if (EnemyManager.instance.currentEnemyStates == EnemyStates.Idle && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            transform.LookAt(target);
            animator.SetTrigger(Animator.StringToHash("Idle"));

            var distance = transform.position.z - PlayerManager.instance.currentPlayer.transform.position.z;
            if (distance < -8)
            {
                EnemyManager.instance.enemyList.Remove(this.gameObject);
            }
        }
    }
    public void Idle()
    {
        if (EnemyManager.instance.currentEnemyStates == EnemyStates.Idle)
        {
            animator.SetBool(Animator.StringToHash("Idle"),true);
        }
    }
    protected void Die()
    {
        animator.SetTrigger(Animator.StringToHash("Die"));
        capsuleCollider.height = capsuleColliderHeight;
        EnemyManager.instance.enemyList.Remove(this.gameObject);
        Destroy(this.gameObject,0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && GameManager.instance.currentGameState != GameManager.GameState.Climax)
        {
            Die();
        }
    }
    public bool HasPlayerPassed()
    {
        var distance = transform.position.z - PlayerManager.instance.currentPlayer.transform.position.z;

        if (distance < 0)
        {
            return true;
        }
        else
        {         
            return false;
        }
    }
    private void DiableEnemy()
    {
        if (HasPlayerPassed())
        {
            if (timeToDisabled > 0)
            {
                timeToDisabled -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
