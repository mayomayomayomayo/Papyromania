using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

using static Mod;

internal static class ModLoader
{
    internal static readonly Dictionary<ModID, Mod> mods = new();

    private static readonly Dictionary<ModID, Manifest> _manifests = new();

    private static readonly string _modsPath = GetModsFolder();

    private static string GetModsFolder()
    {
        string path = Path.Combine(Application.persistentDataPath, "Mods");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        return path;
    }

    internal static async Task LoadMods()
    {
        mods.Clear();

        ModID[] order = await GetModLoadingOrder();

        foreach (ModID id in order)
        {
            mods[id] = LoadMod(id);
        }
    }

    private static Mod LoadMod(ModID id)
    {
        Manifest man = _manifests[id];
        
        string dllPath = Path.Combine(man.folderPath, $"{man.DllName ?? man.ID}.dll");
        
        Assembly asm;
        try
        {
            asm = Assembly.LoadFrom(dllPath);
        }
        catch(Exception e)
        {
            throw new Exception($"Issue when loading mod dll for mod [{man.Name} (ID: {man.ID})]: ", e);
        }

        return new(man, asm);
    }

    private static async Task<ModID[]> GetModLoadingOrder()
    {
        await PopulateManifests();

        List<ModID> orderedMods = new();

        HashSet<ModID> visited = new();
        HashSet<ModID> visiting = new();

        foreach (ModID id in _manifests.Keys)
            Visit(id);
        
        return orderedMods.ToArray();

        void Visit(ModID id)
        {
            if (visited.Contains(id)) return;

            if (visiting.Contains(id))
                throw new Exception($"Circular dependency detected when trying to order mod: [{id}]");

            visiting.Add(id);

            foreach (ModID dep in _manifests[id].Dependencies)
            {
                if (!_manifests.ContainsKey(dep))
                    throw new Exception($"Missing mod dependency: [{id}] requires [{dep}] which was not found");

                Visit(dep);
            }

            visiting.Remove(id);
            visited.Add(id);
            orderedMods.Add(id);
        }
    }

    private static async Task PopulateManifests()
    {
        _manifests.Clear();

        string[] modFolders = Directory.GetDirectories(_modsPath);

        foreach (string folder in modFolders)
        {
            Manifest man = await GetManifest(folder);

            if (_manifests.ContainsKey(man.ID))
                throw new Exception($"Duplicate mod ID found: {man.ID}");
                
            _manifests[man.ID] = man;
        }
    }

    public static async Task<Manifest> GetManifest(string thisModFolder)
    {
        string path = Path.Combine(thisModFolder, "manifest.json");
        if (!File.Exists(path))
            throw new Exception($"Mod folder [{thisModFolder}] has no manifest.json");

        string str = await File.ReadAllTextAsync(path);

        Manifest man;
        try
        {
            man = JsonUtility.FromJson<Manifest>(str);
        }
        catch (Exception e)
        {
            throw new Exception($"Manifest parsing failed for folder [{thisModFolder}]: ", e);
        }
        
        man.folderPath = thisModFolder;
        man.FixMissingInfo();

        return man;
    }
}

public class Mod
{
    public Manifest ModManifest { get; private set; }

    public Assembly ModAssembly { get; private set; }

    public Mod(Manifest man, Assembly asm)
    {
        ModManifest = man;
        ModAssembly = asm;
    }

    public struct Manifest
    {
        public ModID ID { get; private set; }

        public string Name { get; private set; }

        public ModID[] Dependencies { get; private set; }

        public string DllName { get; private set; }

        public string folderPath;

        public void FixMissingInfo()
        {
            if (string.IsNullOrWhiteSpace(Name))
                Name = Path.GetFileName(folderPath);

            Dependencies ??= Array.Empty<ModID>();
        }
    }

    public readonly struct ModID
    {
        private static readonly Regex _regex = new(@"^[a-z0-9\._]+$", RegexOptions.Compiled);

        public string ID { get; }

        public ModID(string id)
        {
            if (!_regex.IsMatch(id))
                throw new Exception($"Invalid mod ID: {id}. Mod IDs can only contain lowercase letters, numbers, periods, and underscores");
            ID = id;
        }

        public static explicit operator ModID(string id) => new(id);
        public static implicit operator string(ModID mid) => mid.ID;
    }
}