using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    public List<ObjectForPool> objectForPools;

    internal Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        //objectForPools = new List<ObjectForPool>();
        AssignInstance();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        if (LevelManager.instance.currentLevelCount < LevelManager.TOTALLEVELCOUNT)
        {
            for (int i = 0; i < LevelManager.instance.levelDatas.Count; i++)
            {
                if(LevelManager.instance.currentLevelCount == i) 
                {
                    for (int j = 0; j < LevelManager.instance.levelDatas[i].objectsInLevels.Count; j++)
                    {
                        objectForPools[j].prefab = LevelManager.instance.levelDatas[i].objectsInLevels[j].sceneObjects[j];
                        objectForPools[j].prefabCount = LevelManager.instance.levelDatas[i].objectsInLevels[j].sceneObjects.Count;
                    }
                }             
            }
        }
        else
        {
            throw new System.Exception("level count above total level count");
        }
     

        //  ObjectPoolInstantiate();
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
    private void ObjectPoolInstantiate()
    {
        foreach (ObjectForPool objectPool in objectForPools)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            for (int i = 0; i < objectPool.prefabCount; i++)
            {
                GameObject gameObject = Instantiate(objectPool.prefab);
                gameObject.transform.parent = this.transform;
                gameObject.SetActive(false);
                poolQueue.Enqueue(gameObject);
            }
            poolDictionary.Add(objectPool.prefab.name, poolQueue);
        }
    }
    public GameObject GetObjectFromPool(GameObject gameObject)
    {
        if (!poolDictionary.ContainsKey(gameObject.name))
        {
            GameObject newGameObject = Instantiate(gameObject);
            newGameObject.name = gameObject.name;
            return newGameObject;
        }
        else
        {
            GameObject objectFromPool = poolDictionary[gameObject.name].Dequeue();

            objectFromPool.SetActive(true);
            poolDictionary[gameObject.name].Enqueue(objectFromPool);

            return objectFromPool;
        }

    }
    public GameObject GetObjectFromPool(string gameObject)
    {
        if (!poolDictionary.ContainsKey(gameObject))
        {
            GameObject data = Resources.Load<GameObject>("Prefabs/" + gameObject);
            if(data != null)
            {
                GameObject newGameObject = Instantiate(data);
                newGameObject.name = gameObject;
                return newGameObject;
            }
            else
            {
                return null;
            }
          
        }
        else
        {
            GameObject objectFromPool = poolDictionary[gameObject].Dequeue();

            objectFromPool.SetActive(true);
            poolDictionary[gameObject].Enqueue(objectFromPool);

            return objectFromPool;
        }

    }
    public void ReturnToObjectPool(GameObject gameObject)
    {
        if (!poolDictionary.ContainsKey(gameObject.name))
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            poolQueue.Enqueue(gameObject);
            gameObject.SetActive(false);

            poolDictionary.Add(gameObject.name, poolQueue);
        }
        else
        {
            gameObject.SetActive(false);
            poolDictionary[gameObject.name].Enqueue(gameObject);
        }
    }
    public void ReturnToObjectPool(GameObject gameObject,string gameobjectName)
    {
        if (!poolDictionary.ContainsKey(gameobjectName))
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            GameObject data = Resources.Load<GameObject>("Prefabs/" + gameobjectName);
            if(data != null)
            {
                poolQueue.Enqueue(data);
                data.SetActive(false);

                poolDictionary.Add(gameobjectName, poolQueue);
            }
            else
            {
                poolQueue.Enqueue(gameObject);
                gameObject.SetActive(false);

                poolDictionary.Add(gameobjectName, poolQueue);
            }
         
        }
        else
        {
            gameObject.SetActive(false);
            poolDictionary[gameobjectName].Enqueue(gameObject);
        }
    }
}
