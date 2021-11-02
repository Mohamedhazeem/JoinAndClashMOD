using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{ 
    public static PlayerManager instance;
    
    [Header("Player Spawn Point")]
    [SerializeField]private Transform playerSpawnPoint;

    [Header("Players")]
    [SerializeField]
    private GameObject player;
    internal GameObject currentPlayer;
    public Material playerMaterial;

    public PlayerStates currentPlayerStates;

    public List<GameObject> npc;
    public LayerMask layerMask;

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
    private void Start()
    {
        currentPlayerStates = PlayerStates.Idle;
    }
  
    public void SwitchPlayerState()
    {
        switch (currentPlayerStates)
        {
            case PlayerStates.Idle:
                currentPlayerStates = PlayerStates.Running;
                break;

            case PlayerStates.Running:
                currentPlayerStates = PlayerStates.Idle;
                break;

            case PlayerStates.Attack:
                break;

            case PlayerStates.Win:
                break;

            case PlayerStates.Die:
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
