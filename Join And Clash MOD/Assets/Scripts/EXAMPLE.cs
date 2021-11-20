using UnityEngine;
using System.Collections.Generic;
public class EXAMPLE : MonoBehaviour
{ 
    public int totalLevelCount;

    public int currentLevelCount = 0;


    public List<int> levelData = new List<int>();
    private void Start()
    {
        totalLevelCount = LevelManager.TOTALLEVELCOUNT;
    }
    public void IncreaseLastSaveLevelCount()
    {
        ++currentLevelCount;
    }
}
