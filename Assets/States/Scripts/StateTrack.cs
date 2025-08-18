using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class StateTrack : MonoBehaviour
{
    public static StateTrack Instance;

    private static readonly char sep = Path.DirectorySeparatorChar;
    private static string SaveDirectory => $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{sep}NorthernIsles{sep}Save";
    private string GetLocation(string saveName) => $"{SaveDirectory}{sep}{saveName}.json";

    private SortedList<int, IStateProvider> orderedProviders;
    private Dictionary<string, IStateProvider> indexedProviders;

    public UnityEvent OnBeginLoadState;
    public UnityEvent OnBeginRollback;

    private StateTrack()
    {
        Instance = this;
        indexedProviders = new();
        orderedProviders = new();
        OnBeginLoadState ??= new();
        OnBeginRollback ??= new();
    }

    public IEnumerable<(string name, DateTime lastWrite)> AvailableSaves =>
        Directory.Exists(SaveDirectory) ?
            Directory.EnumerateFiles(SaveDirectory).Select(path => (Path.GetFileNameWithoutExtension(path), File.GetLastWriteTimeUtc(path))) :
            new List<(string, DateTime)>();

    public void AddProvider(IStateProvider provider)
    {
        orderedProviders.Add(provider.GetPriority(), provider);
        indexedProviders.Add(provider.GetName(), provider);
    }

    public void SaveQuickState() => SaveState($"QuickSave_{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}");


    [Button()]
    public void SaveState(string saveName)
    {
        var save = SaveStructure.Create();
        foreach (var provider in orderedProviders.Values)
            save.Add(provider.GetName(), provider.GetState());

        string location = GetLocation(saveName);
        FileInfo info = new(location);
        if (!info.Exists)
            Directory.CreateDirectory(info.Directory.FullName);
        File.WriteAllText(location, JsonConvert.SerializeObject(save, Formatting.Indented));
    }

    [Button()]
    public void LoadState(string saveName)
    {
        SpeedControl.Instance.Pause();

        OnBeginLoadState.Invoke();

        string location = GetLocation(saveName);
        var save = JsonConvert.DeserializeObject<SaveStructure>(File.ReadAllText(location));
        foreach (var state in save.states)
            indexedProviders[state.name].SetState(state.data);
    }


    [Button()]
    public void LoadResourceState(string saveName)
    {
        SpeedControl.Instance.Pause();

        OnBeginLoadState.Invoke();

        var jsonAsset = Resources.Load<TextAsset>(saveName);
        var save = JsonConvert.DeserializeObject<SaveStructure>(jsonAsset.ToString());
        foreach (var state in save.states)
            indexedProviders[state.name].SetState(state.data);
    }

    [Button()]
    public void Rollback()
    {
        SpeedControl.Instance.Pause();

        OnBeginRollback.Invoke();

        foreach (var provider in orderedProviders.Values)
            provider.Rollback();
    }

    [Serializable]
    private struct SaveStructure
    {
        public List<ProviderState> states;

        public static SaveStructure Create()
        {
            return new()
            {
                states = new List<ProviderState>()
            };
        }

        public void Add(string providerName, JToken providerData)
        {
            states.Add(new()
            {
                name = providerName,
                data = providerData
            });
        }

        [Serializable]
        public struct ProviderState
        {
            public string name;
            public JToken data;
        }
    }
}

public interface IStateProvider
{
    public string GetName();
    public int GetPriority();

    public JToken GetState();
    public void SetState(JToken element);
    public void Rollback();
}
