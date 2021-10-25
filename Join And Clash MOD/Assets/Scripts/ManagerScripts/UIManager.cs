using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public enum MenuState
    {
        BeforeStart,
        Started
    }
    public MenuState currentMenuState;

    public GameObject HoldAndDrag_Text;

    private void Awake()
    {
        AssignInstance();
    }
    private void Start()
    {
        currentMenuState = MenuState.BeforeStart;
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

    public void SwitchUiState()
    {
        switch (currentMenuState)
        {
            case MenuState.BeforeStart:
                currentMenuState = MenuState.Started;
                break;

            case MenuState.Started:
                break;

            default:
                break;
        }
    }
}
