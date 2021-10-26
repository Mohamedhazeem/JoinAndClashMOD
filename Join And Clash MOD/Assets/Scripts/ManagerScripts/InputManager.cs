using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public delegate void MouseCallback();
    public event MouseCallback OnMouseHold, OnMouseDown, OnMouseUp;

    public delegate void MouseDragCallback(float X);
    public event MouseDragCallback OnMouseDrag;

    [Header("Ortho Graphic Camera")]
    [SerializeField] private Camera orthographicCamera;

    private Vector3 MouseStartPosition;
     private Vector3 MouseCurrentPosition;

   
    private void Awake()
    {
        AssignInstance();
    }
    private void AssignInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    void Update()
    {
        if(GameManager.instance.currentGameState == GameManager.GameState.Menu && UIManager.instance.currentMenuState == UIManager.MenuState.BeforeStart && PlayerManager.instance.currentPlayerStates == PlayerManager.PlayerStates.Idle && Input.GetMouseButtonDown(0))
        {
            GameManager.instance.SwitchState();
            UIManager.instance.SwitchUiState();
        }

        if (GameManager.instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.instance.currentPlayerStates == PlayerManager.PlayerStates.Idle && Input.GetMouseButtonDown(0))
        {
            PlayerManager.instance.SwitchPlayerState();
            OnMouseDown?.Invoke();
            MouseStartPosition = orthographicCamera.ScreenToWorldPoint(Input.mousePosition);
            MouseStartPosition.y = PlayerManager.instance.currentPlayer.transform.position.y;

        }
        else if (Input.GetMouseButton(0) && GameManager.instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.instance.currentPlayerStates == PlayerManager.PlayerStates.Running)
        {
            OnMouseHold?.Invoke();

            MouseCurrentPosition = orthographicCamera.ScreenToWorldPoint(Input.mousePosition);
            MouseCurrentPosition.y = PlayerManager.instance.currentPlayer.transform.position.y;

            var difference = MouseCurrentPosition - MouseStartPosition;
            OnMouseDrag(difference.x);
         
        }
        else if(Input.GetMouseButtonUp(0) && GameManager.instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.instance.currentPlayerStates == PlayerManager.PlayerStates.Running)
        {

            PlayerManager.instance.SwitchPlayerState();
            OnMouseUp?.Invoke();           
        }

    }
  
}

