using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blade : ObstaclesAnimationManager
{
    public Vector3 end;
    public float duration;
    void Start()
    {
       // ObstaclesAnimation();
        transform.DORotate(end, duration,RotateMode.LocalAxisAdd).SetLoops(-1,LoopType.Incremental).SetEase(Ease.Linear);
    }
    void Update()
    {
        
    }
}
