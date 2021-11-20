using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    
    public const int TOTALLEVELCOUNT = 2;

    public List<LevelData> levelDatas = new List<LevelData>();
    public int levelDataCount;
    void Awake()
    {
        levelDataCount = levelDatas.Count;
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

        string load = SaveLevelDesign.Load();
        List<LevelData> data = JsonHelper.ListFromJson<LevelData>(load);

        AssignLevelData(data);

        levelDataCount = levelDatas.Count;
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
        public List<string> SceneObjectsName;
        public List<Vector3> sceneObjectTransform;
        public void AssignSceneObjectsName()
        {
            SceneObjectsName.Add(sceneObjects[0].name);
            //for (int i = 0; i < sceneObjects.Count; i++)
            //{
            //    SceneObjectsName.Add(sceneObjects[i].name);
            //}
        }
        public void AssignSceneObjectsTransform()
        {            
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                sceneObjectTransform.Add(sceneObjects[i].transform.position);
            }
        }
    }


}

