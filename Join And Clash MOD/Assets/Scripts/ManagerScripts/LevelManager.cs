using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public string Text;
    public int currentLevelCount;
    public const int TOTALLEVELCOUNT = 2;

    public List<LevelData> levelDatas;

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

        AssignSceneObjectsName();

        string load = SaveLevelDesign.Load();

        List<LevelData> data = JsonHelper.ListFromJson<LevelData>(load);

        AssignLevelData(data);
    }
    public void AssignSceneObjectsName()
    {

        for (int i = 0; i < levelDatas.Count; i++)
        {
            for (int j = 0; j < levelDatas[i].objectsInLevels.Count; j++)
            {
                levelDatas[i].objectsInLevels[j].AssignSceneObjectsName();
                levelDatas[i].objectsInLevels[j].AssignSceneObjectsTransform();
            }

        }
    }
    public void AssignLevelData (List<LevelData> data)
    {
        for (int i = 0; i < levelDatas.Count; i++)
        {
            levelDatas[i] = data[i];
        }
    }
    public void Asign()
    {
           
        var v = JsonHelper.ToJson(levelDatas, true);
        SaveLevelDesign.Save(v);
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
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                SceneObjectsName.Add(sceneObjects[i].name);
            }
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
