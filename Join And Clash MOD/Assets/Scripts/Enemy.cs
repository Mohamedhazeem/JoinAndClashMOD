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
        //transform.position  = Vector3.MoveTowards(transform.position,)
    }
    protected void StartRunAnimation()
    {
        if (PlayerManager.instance.currentPlayerStates == PlayerStates.Running && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            animator.SetTrigger(Animator.StringToHash("Run"));
        }
    }
    protected virtual void Move()
    {
        //vecto3.forward
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
    }
    protected void StopMove()
    {
        if (PlayerManager.instance.currentPlayerStates == PlayerStates.Idle && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            animator.SetTrigger(Animator.StringToHash("Idle"));
        }
    }
    protected void Die()
    {
        animator.SetTrigger(Animator.StringToHash("Die"));
        capsuleCollider.height = capsuleColliderHeight;
        Destroy(this.gameObject, 1f);
    }
    //protected virtual void PlayerSideMoves(float x)
    //{
    //    transform.Translate(Vector3.right * x * xMoveSpeed * Time.deltaTime, Space.World);
    //    var position = transform.position;
    //    position.x = Mathf.Clamp(position.x, xMinimum, xMaximum);
    //    transform.position = position;
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Die();
        }
    }

}
