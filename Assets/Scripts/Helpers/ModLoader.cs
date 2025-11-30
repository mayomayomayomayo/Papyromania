using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using UnityEngine;

public static class ModLoader
{
    public static Dictionary<string, Type> knownCustomCardBehaviours = new();

    public static void LoadAllMods()
    {
        string modFolderPath = Path.Combine(Application.persistentDataPath, "Mods");
        if (!Directory.Exists(modFolderPath)) Directory.CreateDirectory(modFolderPath);

        foreach (string dllPath in Directory.GetFiles(modFolderPath, "*.dll")) GetCustomCardBehaviours(Assembly.LoadFrom(dllPath));
    }

    public static void GetCustomCardBehaviours(Assembly mod)
    {
        IEnumerable<Type> customCardBehaviours = mod.GetTypes().Where(t => t.IsSubclassOf(typeof(CustomCardBehaviour)) && !t.IsAbstract);

        foreach (Type t in customCardBehaviours) knownCustomCardBehaviours[t.Name] = t;
    }
}


