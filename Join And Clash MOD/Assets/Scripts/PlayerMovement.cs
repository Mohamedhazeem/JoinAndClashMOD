using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;
    [Header("Move Speed")]
    [SerializeField] private float moveSpeed;
    [Header("Clamp Value on X Axis")]
    [SerializeField] private float xMinimum,xMaximum;
    [Header("Move speed on X Axis")]
    [SerializeField] private float xMoveSpeed;

    private void Start()
    {
        InputManager.instance.OnMouseHold += Move;
        InputManager.instance.OnMouseUp += StopMove;
        InputManager.instance.OnMouseDown += StartRunAnimation;
        InputManager.instance.OnMouseDrag += PlayerSideMoves;
    }

    private void StartRunAnimation()
    {
        if (PlayerManager.instance.currentPlayerStates == PlayerManager.PlayerStates.Running && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            // playerAnimator.ResetTrigger(Animator.StringToHash("Idle"));
            playerAnimator.SetTrigger(Animator.StringToHash("Run"));
        }
    }

    private void Move()
    {        
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
    }
    private void StopMove()
    {
        if (PlayerManager.instance.currentPlayerStates == PlayerManager.PlayerStates.Idle && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
           // playerAnimator.ResetTrigger(Animator.StringToHash("Run"));
            playerAnimator.SetTrigger(Animator.StringToHash("Idle"));
        }
    }
    private void PlayerSideMoves(float x)
    {
        transform.Translate(Vector3.right * x* xMoveSpeed * Time.deltaTime, Space.World);
        var position = transform.position;
        position.x = Mathf.Clamp(position.x, xMinimum, xMaximum);
        transform.position = position;
    }
 
}
