using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitchManager : MonoBehaviour
{
    public static SceneSwitchManager instance;
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
    public void RestartScene()
    {
        SceneManager.LoadScene(UserDataManager.instance.currentLevelCount);
    }
    public void NextScene()
    {
        if(UserDataManager.instance.currentLevelCount < LevelManager.TOTALLEVELCOUNT)
        {
            SceneManager.LoadScene(UserDataManager.instance.currentLevelCount);
        }
        else
        {
            UserDataManager.instance.ResetCurrentLevelCount();
            SceneManager.LoadScene(UserDataManager.instance.currentLevelCount);
        }
       
    }
}
