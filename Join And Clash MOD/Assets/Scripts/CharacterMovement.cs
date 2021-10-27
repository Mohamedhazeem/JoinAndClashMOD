using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : PlayerMovement
{
    [Header("IS MOVE")]
    public bool isMove;
    [Header("Renderer")]
    public Renderer characterRenderer;
    protected override void Move()
    {
        if (isMove)
        {
            base.Move();
            
        }        
    }
    protected override void PlayerSideMoves(float x)
    {
        if (isMove)
        {
            base.PlayerSideMoves(x);
        }        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isMove = true;
            transform.tag = collision.transform.tag;
            animator.SetTrigger(Animator.StringToHash("ToIdle"));
            characterRenderer.material = PlayerManager.instance.playerMaterial;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMove = true;
            transform.tag = other.transform.tag;
            animator.SetTrigger(Animator.StringToHash("ToIdle"));
            characterRenderer.material = PlayerManager.instance.playerMaterial;
        }
    }
}
