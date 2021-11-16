using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
     public enum GameState
     {
        Menu,
        GamePlay,
        Climax,
        Win,
        Lose
     }
    
    public GameState currentGameState;

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

    private void Start()
    {
        currentGameState = GameState.Menu;
    }
    public void SwitchGameStates()
    {
        switch (currentGameState)
        {
            case GameState.Menu:
                currentGameState = GameState.GamePlay;
                break;

            case GameState.GamePlay:
                break;

            case GameState.Win:
                UserDataManager.instance.SaveCurrentLevelCount();
                UIManager.instance.NextSceneButtonSetActive();
                break;

            case GameState.Lose:
                UIManager.instance.RestartSceneButtonSetActive();
                break;

            case GameState.Climax:
                break;

            default:
                currentGameState = GameState.Menu;
                break;
        }
    }
}
