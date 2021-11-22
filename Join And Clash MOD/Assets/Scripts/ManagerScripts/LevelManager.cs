using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    
    public const int TOTALLEVELCOUNT = 2;

    public List<LevelData> levelDatas;
    [HideInInspector]
    public List<LevelData> data;
    void Awake()
    {
        AssignInstance();
        LoadLevelDesign();
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
    private void LoadLevelDesign()
    {
        SaveLevelDesign.Init();
        AssignSceneObjectsNameAndTransform();

        string load = SaveLevelDesign.Load();

        data = JsonHelper.ListFromJson<LevelData>(load);

        //AssignLevelData(data);
    }

    public void AssignLevelData(List<LevelData> data)
    {
        for (int i = 0; i < TOTALLEVELCOUNT; i++)
        {
            if(UserDataManager.instance.currentLevelCount == i)
            {
                levelDatas[i] = data[i];
            }
            
        }
    }
    public void SaveLevelData()
    {
        var levelData = JsonHelper.ToJson(levelDatas, true);
        SaveLevelDesign.Save(levelData);
    }
    public void AssignSceneObjectsNameAndTransform()
    {
        for (int i = 0; i < levelDatas.Count; i++)
        {
            for (int j = 0; j < levelDatas[i].objectsInLevels.Count; j++)
            {
                levelDatas[i].objectsInLevels[j].AssignSceneObjectsTransform();
            }
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public string name;
        public List<ObjectsInLevel> objectsInLevels;

    }

    [System.Serializable]
    public struct ObjectsInLevel
    {
        public string name;
        public List<GameObject> sceneObjects;
        public string SceneObjectName;
        public List<Vector3> sceneObjectTransform;

        public void AssignSceneObjectsTransform()
        {            
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                sceneObjectTransform.Add(sceneObjects[i].transform.position);
            }
        }
    }


}

