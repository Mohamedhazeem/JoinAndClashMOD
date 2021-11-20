using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public delegate void ClimaxWinAnimationCallback();
    public event ClimaxWinAnimationCallback OnClimaxWinAnimation;

    public EnemyStates currentEnemyStates;
    int enemySpawnCount = 1;
    private delegate void SpawnEnemiesCallBack();
    private SpawnEnemiesCallBack spawnEnemies;

    public float enemySpawnTime;
    private WaitForSeconds waitForSeconds;

    public List<GameObject> enemyList;

    public GameObject enemyPrefab;
    public GameObject enemiesSpawnPoint;

    public bool isBossFight;
    public BossEnemy bossEnemy;
    public Transform bossEnemyTransform;
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
        else if(instance != this)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        spawnEnemies = SpawnEnemies;
        if(bossEnemy != null)
        {
            bossEnemyTransform = bossEnemy.transform;
        }       
        currentEnemyStates = EnemyStates.Idle;
        waitForSeconds = new WaitForSeconds(enemySpawnTime);
    }
    public void StartSpawningEnemies()
    {
        if (spawnEnemies != null)
        {
            spawnEnemies();
        }
    }
    public void SpawnEnemies()
    {
        var randomNumber = Random.Range(8, 15);
        if (PlayerManager.instance.npc.Count > 2)
        {
            StartCoroutine(SpawnEnemiesCouroutine(randomNumber));         
        }
        else
        {
            StartCoroutine(SpawnEnemiesCouroutine(5));
        }       
    }
    IEnumerator SpawnEnemiesCouroutine(int enemyCount)
    {
        currentEnemyStates = EnemyStates.Idle;
        while (enemySpawnCount < enemyCount)
        {
            enemySpawnCount++;
            yield return waitForSeconds;
            GameObject gameObject = Instantiate(enemyPrefab, enemiesSpawnPoint.transform.position, Quaternion.Euler(0, 180, 0));
            enemyList.Add(gameObject);
            var castleEnemy = gameObject.GetComponent<CastleEnemy>();
            var playerTransform = PlayerManager.instance.PlayerAndNPCTransform();
            var player = playerTransform.GetComponent<Player>();

            castleEnemy.targetTransform = playerTransform;
            castleEnemy.player = player;
       

            player.castleEnemy = castleEnemy;
            player.targetTransform = EnemyTransform();
            player.isTargetAvailable = true;
        }

    }
    public void AssignToBoss()
    {

        if (PlayerManager.instance.npc.Count >= 0) 
        {
            for (int i = 0; i <= PlayerManager.instance.npc.Count; i++)
            {
                var targetTransform = PlayerManager.instance.PlayerAndNPCTransform();
                var player = targetTransform.GetComponent<Player>();
                bossEnemy.isTargetAvailable = true;

                bossEnemy.targetTransform = targetTransform;
                bossEnemy.player = player;

                player.targetTransform = bossEnemyTransform;
                player.bossEnemy = bossEnemy;
                player.isTargetAvailable = true;
            }
        }
    }
    public Transform EnemyTransform()
    {
        if (enemyList.Count > 0 )
        {
            var transform = Random.Range(0, enemyList.Count);
            return enemyList[transform].transform;
        }
        else
        {
            return null;
        }
    }
    public void SwitchEnemyStates()
    {
        switch (currentEnemyStates)
        {
            case EnemyStates.Idle:

                break;
            case EnemyStates.Move:

                break;
            case EnemyStates.Chase:

                break;

            case EnemyStates.Attack:

                break;

            case EnemyStates.Win:
                OnClimaxWinAnimation?.Invoke();
                break;

            case EnemyStates.Die:
                break;

            
            default:
                break;
        }
    }
}
public enum EnemyStates
{
    Idle,
    Move,
    Chase,
    Attack,
    Win,
    Die
}

