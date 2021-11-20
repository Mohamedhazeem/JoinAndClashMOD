using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnManager : MonoBehaviour
{
    public static LevelSpawnManager instance;
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
       // SpawnLevel();
    }
    public void SpawnLevel()
    {
        for (int i = 0; i < LevelManager.instance.levelDatas.Count; i++)
        { 
            if (UserDataManager.instance.currentLevelCount == i)
            {
                var count = LevelManager.instance.levelDatas[i].objectsInLevels.Count;

                for (int j = 0; j < count; j++)
                {

                    for (int k = 0; k < LevelManager.instance.levelDatas[i].objectsInLevels[j].sceneObjects.Count; k++)
                    {
                       GameObject gameObject =  ObjectPoolManager.instance.GetObjectFromPool(LevelManager.instance.levelDatas[i].objectsInLevels[j].SceneObjectsName[k]);

                       gameObject.transform.position = LevelManager.instance.levelDatas[i].objectsInLevels[j].sceneObjectTransform[k];
                        Debug.Log(gameObject.transform.position);
                        if(gameObject.name == "EnemySpawnPoint")
                        {
                            EnemyManager.instance.enemiesSpawnPoint = gameObject;
                        }
                        else if(gameObject.name == "PlayerSpawnPoint")
                        {
                            PlayerManager.instance.playerSpawnPoint = gameObject.transform;
                        }
                        else if (gameObject.CompareTag("BossEnemy"))
                        {
                            EnemyManager.instance.isBossFight = true;
                            EnemyManager.instance.bossEnemy = gameObject.GetComponent<BossEnemy>();
                        }
                    }

                }
            }
        }
    }

}
