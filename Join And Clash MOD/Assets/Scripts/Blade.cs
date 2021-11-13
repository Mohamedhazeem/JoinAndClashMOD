using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blade : MonoBehaviour
{
    public Vector3 end;
    public float duration;
    void Start()
    {
        transform.DORotate(end, duration,RotateMode.LocalAxisAdd).SetLoops(-1,LoopType.Incremental).SetEase(Ease.Linear);
    }
}
