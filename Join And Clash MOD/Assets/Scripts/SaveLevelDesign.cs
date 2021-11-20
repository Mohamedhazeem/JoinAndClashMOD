using System.IO;
using UnityEngine;

public static class SaveLevelDesign
{
    
    public static readonly string path = Application.dataPath + "/Saves/";


    public static void Init()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    public static void Save(string saveString)
    {        
        File.WriteAllText(path + "tenporarySave.json",saveString);
    }
 
    public static string Load()
    {
        if (File.Exists(path + "tenporarySave.json"))
        {
            var s = File.ReadAllText(path + "tenporarySave.json");
            return s;
        }
        else
        {
            return null;
        }
    }

}
