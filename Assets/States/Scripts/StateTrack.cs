using System;
using System.Collections.Generic;
using System.IO;
using com.cyborgAssets.inspectorButtonPro;
using Newtonsoft.Json;
using UnityEngine;

public class StateTrack : MonoBehaviour
{
    public static StateTrack Instance;

    private static readonly char sep = Path.DirectorySeparatorChar;
    private string location = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{sep}NorthernIsles{sep}save.txt";

    private SortedList<int, IStateProvider> orderedProviders;
    private Dictionary<string, IStateProvider> indexedProviders;

    private StateTrack()
    {
        Instance = this;
        indexedProviders = new();
        orderedProviders = new();
    }

    public void AddProvider(IStateProvider provider)
    {
        orderedProviders.Add(provider.GetPriority(), provider);
        indexedProviders.Add(provider.GetName(), provider);
    }

    [ProButton]
    public void SaveState()
    {
        var save = SaveStructure.Create();
        foreach (var provider in orderedProviders.Values)
            save.Add(provider.GetName(), provider.GetState());

        FileInfo info = new(location);
        if (!info.Exists)
            Directory.CreateDirectory(info.Directory.FullName);
        File.WriteAllText(location, JsonUtility.ToJson(save));
    }

    [ProButton]
    public void LoadState()
    {
        var save = JsonUtility.FromJson<SaveStructure>(File.ReadAllText(location));
        foreach (var state in save.states)
            indexedProviders[state.name].SetState(state.data);
    }

    [ProButton]
    public void Rollback()
    {
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

        public void Add(string providerName, string providerData)
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
            public string data;
        }
    }
}

public interface IStateProvider
{
    public string GetName();
    public int GetPriority();

    public string GetState();
    public void SetState(string json);
    public void Rollback();
}
