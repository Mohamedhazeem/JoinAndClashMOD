using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    //public LayerMask layerMask;
    [Header("Player Spawn Point")]
    [SerializeField] Transform playerSpawnPoint;
    [Header("Players")]
    [SerializeField] GameObject player;

    internal GameObject currentPlayer;

    public PlayerStates playerStates;
    private void Awake()
    {
        AssignInstance();
        currentPlayer = Instantiate(player, playerSpawnPoint.position, Quaternion.identity);
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
    public void SwitchPlayerState()
    {
        switch (playerStates)
        {
            case PlayerStates.Idle:
                playerStates = PlayerStates.Running;
                break;

            case PlayerStates.Running:
                break;

            case PlayerStates.Attack:
                break;

            case PlayerStates.Win:
                break;

            case PlayerStates.Die:
                break;

            default:
                playerStates = PlayerStates.Idle;
                break;
        }
    }
}
public enum PlayerStates
{
    Idle,
    Running,
    Attack,
    Win,
    Die
}