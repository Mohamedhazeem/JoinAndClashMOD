using System.Collections.Generic;
using UnityEngine;
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public EnemyStates currentEnemyStates;

    public List<GameObject> enemyList;
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
        currentEnemyStates = EnemyStates.Idle;
    }
}
public enum EnemyStates
{
    Idle,
    Chase,
    Attack,
    Win,
    Die
}