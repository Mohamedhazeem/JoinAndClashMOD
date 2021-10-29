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

    public void SpawnLevel()
    {

    }

}
