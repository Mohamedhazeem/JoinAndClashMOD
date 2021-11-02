using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstaclesAnimationManager : MonoBehaviour
{
    public ObstacleAnimationsState primaryObstacleAnimationsState;
    public ObstacleAnimationsState secondaryObstacleAnimationsState;

    public void ObstaclesAnimation()
    {
        switch (primaryObstacleAnimationsState)
        {
            case ObstacleAnimationsState.None:
                Debug.Log("Working none");
                break;
            case ObstacleAnimationsState.Rotate:
               // transform.DORotate(Vector3.up*360,-1, RotateMode.WorldAxisAdd).SetLoops(-1);
                Debug.Log("Working rotate");
                break;
            case ObstacleAnimationsState.Move:
                Debug.Log("Working move");
                break;
            default:
                break;
        }
        switch (secondaryObstacleAnimationsState)
        {
            case ObstacleAnimationsState.None:
                Debug.Log("Working none");
                break;
            case ObstacleAnimationsState.Rotate:
                Debug.Log("Working rotate");
                break;
            case ObstacleAnimationsState.Move:
                Debug.Log("Working move");
                break;
            default:
                break;
        }
    }
    void Update()
    {
        
    }
}
public enum ObstacleAnimationsState
{
    None,
    Rotate,
    Move
}
