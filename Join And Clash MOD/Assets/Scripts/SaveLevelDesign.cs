﻿using System.IO;
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
        File.WriteAllText(path + "jsonsample.json",saveString);
    }
    public static string Load()
    {
        if (File.Exists(path + "jsonsample.json"))
        {
            var s = File.ReadAllText(path + "jsonsample.json");
            return s;
        }
        else
        {
            return null;
        }
    }
}
