using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{ 
    public static PlayerManager instance;

    public delegate void ClimaxIdleAnimationCallback();
    public event ClimaxIdleAnimationCallback OnClimaxIdleAnimation;

    public delegate void ClimaxWinAnimationCallback();
    public event ClimaxWinAnimationCallback OnClimaxWinAnimation;

    [Header("Player Spawn Point")]
    public Transform playerSpawnPoint;

    [Header("Players")]
    [SerializeField]
    private GameObject player;
    public GameObject currentPlayer;
    public Material playerMaterial;

    public PlayerStates currentPlayerStates;

    public List<GameObject> npc;
    public float enemyrange;
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
                //if(GameManager.instance.currentGameState != GameManager.GameState.Climax)
                //{
                //    currentPlayerStates = PlayerStates.Running;
                //}
                //else
                //{
                    currentPlayerStates = PlayerStates.Idle;
                //}
                
                break;

            case PlayerStates.Attack:

                break;

            case PlayerStates.Win:
                OnClimaxWinAnimation?.Invoke();
                break;

            case PlayerStates.Die:
                break;

            case PlayerStates.ClimaxIdle:
                StartCoroutine(ClimaxIdleToRun());
                OnClimaxIdleAnimation.Invoke();
                Debug.Log("call");
                break;

            default:
                break;
        }
    }
    IEnumerator ClimaxIdleToRun()
    {
        yield return new WaitForSeconds(1f);
        currentPlayerStates = PlayerStates.Attack;
    }
    public Transform PlayerAndNPCTransform()
    {
        if (npc.Count > 0 && currentPlayer.activeInHierarchy)
        {          
            if (!npc.Contains(currentPlayer))
            {
                npc.Add(currentPlayer);
            }
            
            var transform = Random.Range(0, npc.Count);
            return npc[transform].transform;            
        }
        else if (npc.Count > 0 && !currentPlayer.activeInHierarchy)
        {
            var transform = Random.Range(0, npc.Count);
            return npc[transform].transform;
        }
        else if(npc.Count == 0 && currentPlayer.activeInHierarchy)
        {
            return currentPlayer.transform;
        }
        else
        {
            return null;
        }
    }
}
public enum PlayerStates
{
    Idle,
    Running,
    Attack,
    ClimaxIdle,
    Win,
    Die
}
