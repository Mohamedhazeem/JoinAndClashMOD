using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public EnemyStates currentEnemyStates;
    int enemySpawnCount = 1;
    private delegate void SpawnEnemiesCallBack();
    private SpawnEnemiesCallBack spawnEnemies;

    public float enemySpawnTime;
    private WaitForSeconds waitForSeconds;

    public List<GameObject> enemyList;

    public GameObject enemyPrefab;
    public GameObject enemiesSpawnPoint;
    private Vector3 enemyPositionOnSpawnPoint = new Vector3();
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
        var randomNumber = Random.Range(2, 8);
        if(PlayerManager.instance.npc.Count > 2)
        {
            var enemyCount = PlayerManager.instance.npc.Count - randomNumber;
            enemyCount = Mathf.Abs(enemyCount);
            StartCoroutine(SpawnEnemiesCouroutine(enemyCount));
        }
        else
        {
            StartCoroutine(SpawnEnemiesCouroutine(5));
        }
       
    }
    IEnumerator SpawnEnemiesCouroutine(int enemyCount)
    {
        while (enemySpawnCount < enemyCount)
        {
            enemySpawnCount++;
            yield return waitForSeconds;
            GameObject gameObject = Instantiate(enemyPrefab, enemiesSpawnPoint.transform.position, Quaternion.Euler(0, 180, 0));
            var castleEnemy = gameObject.GetComponent<CastleEnemy>();
            castleEnemy.targetTransform = PlayerManager.instance.PlayerAndNPCTransform();
            currentEnemyStates = EnemyStates.Chase;           
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

