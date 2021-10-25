using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public delegate void MouseHoldCallback();
    public event MouseHoldCallback OnMouseHold;
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
        if(GameManager.instance.currentGameState == GameManager.GameState.Menu && UIManager.instance.currentMenuState == UIManager.MenuState.BeforeStart && Input.GetMouseButtonDown(0))
        {
            GameManager.instance.SwitchState();
            UIManager.instance.SwitchUiState();
        }
        else if (Input.GetMouseButton(0) && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            OnMouseHold?.Invoke();
        }
    }
}
