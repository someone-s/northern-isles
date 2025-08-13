using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class RegionDatabase : MonoBehaviour, IStateProvider
{
    public static RegionDatabase Instance { get; private set; }

    private Dictionary<string, Region> regions;
    public IReadOnlyDictionary<string, Region> Regions => regions;

    public string GetName() => "RegionDatabase";
    public int GetPriority() => 6;

    private RegionDatabase()
    {
        Instance = this;
    }

    private void Awake()
    {
        regions = GetComponentsInChildren<Region>().ToDictionary(keySelector: region => region.name, elementSelector: region => region);
        StateTrack.Instance.AddProvider(this);
    }

    public JToken GetState()
    {
        return JToken.FromObject(new DatabaseState()
        {
            regions = regions.Select(pair => new RegionPair() { name = pair.Key, data = pair.Value.GetState() }).ToList()
        });
    }

    public void SetState(JToken json)
    {
        var state = json.ToObject<DatabaseState>();
        foreach (var pair in state.regions)
            regions[pair.name].SetState(pair.data);
    }

    public void Rollback()
    {
        foreach (var region in regions.Values)
            region.Rollback();
    }

    [SerializeField]
    private struct DatabaseState
    {
        public List<RegionPair> regions;
    }

    [SerializeField]
    private struct RegionPair
    {
        public string name;
        public JToken data;
    }
}