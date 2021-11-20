using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    public List<ObjectForPool> objectForPools;
    internal Dictionary<string, Queue<GameObject>> poolDictionary;
    void Awake()
    {
        
        AssignInstance();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        if (UserDataManager.instance.currentLevelCount < LevelManager.TOTALLEVELCOUNT)
        {
            AssignObjectForPool();
        }
        else
        {
            UserDataManager.instance.ResetCurrentLevelCount();
            AssignObjectForPool();
        }


         ObjectPoolInstantiate();
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
    private void AssignObjectForPool()
    {      
        

        for (int i = 0; i < LevelManager.TOTALLEVELCOUNT; i++)
        {
            if (UserDataManager.instance.currentLevelCount == i)
            {
                var count = LevelManager.instance.levelDatas[0].objectsInLevels.Count;

                for (int j = 0; j < count; j++)
                {
                    ObjectForPool objectPool = new ObjectForPool();
                    for (int k = 0; k < 1; k++)
                    {
                        var name = LevelManager.instance.levelDatas[0].objectsInLevels[j].SceneObjectsName[k];
                        var prefab = Resources.Load<GameObject>("Prefabs/Floor");
                        Debug.Log(prefab.name);
                        objectPool.prefab = prefab;
                        objectPool.prefabCount = LevelManager.instance.levelDatas[0].objectsInLevels[j].sceneObjects.Count;
                        objectForPools.Add(objectPool);
                    }                    
                }
            }
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
