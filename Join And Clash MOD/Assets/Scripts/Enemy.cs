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


    //[Header("Clamp Value on X Axis")]
    //[SerializeField] private float xMinimum, xMaximum;
    //[Header("Move speed on X Axis")]
    //[SerializeField] private float xMoveSpeed;

    protected virtual void Start()
    {
        //InputManager.instance.OnMouseHold += Move;
        //InputManager.instance.OnMouseUp += StopMove;
        //InputManager.instance.OnMouseDown += StartRunAnimation;
        //InputManager.instance.OnMouseDrag += PlayerSideMoves;

        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    private void Update()
    {
        if (isMove)
        {
            Move();
        }
        //transform.position  = Vector3.MoveTowards(transform.position,)
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
    protected void StopMove()
    {
        if (EnemyManager.instance.currentEnemyStates == EnemyStates.Idle && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            animator.SetTrigger(Animator.StringToHash("Idle"));
        }
    }
    protected void Die()
    {
        animator.SetTrigger(Animator.StringToHash("Die"));
        capsuleCollider.height = capsuleColliderHeight;
        EnemyManager.instance.enemyList.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Die();
        }
    }
    //public bool HasPlayerPassed()
    //{
    //    var distance = transform.position.z -  
    //}
}
