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
    internal bool isMove;
    protected virtual void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    private void Update()
    {
        if (isMove)
        {
            Move();
        }     
    }
    protected void StartRunAnimation()
    {
        if (EnemyManager.instance.currentEnemyStates == EnemyStates.Chase && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            animator.SetTrigger(Animator.StringToHash("Run"));
        }
    }
    public void Move()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        StartRunAnimation();
    }
    public void Chase(Transform target)
    {
        transform.LookAt(target);
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        StartRunAnimation();            
    }
    public void StopMove(Transform target)
    {
        if (EnemyManager.instance.currentEnemyStates == EnemyStates.Idle && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            transform.LookAt(target);
            transform.forward = Vector3.zero;
            animator.SetTrigger(Animator.StringToHash("Idle"));
        }
    }
    protected void Die()
    {
        animator.SetTrigger(Animator.StringToHash("Die"));
        capsuleCollider.height = capsuleColliderHeight;
        EnemyManager.instance.enemyList.Remove(this.gameObject);
        Destroy(this.gameObject,1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
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
}
