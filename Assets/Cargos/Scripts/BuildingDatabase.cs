using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class BuildingDatabase : MonoBehaviour, IStateProvider
{


    [Serializable]
    private struct DatabaseState
    {
        public List<BuildingPair> states;
    }

    [Serializable]
    private struct BuildingPair
    {
        public string guid;
        public BuildingType type;
        public string port;
        public string data;
    }

    
    [SerializeField] private List<GameObject> buildingPrefabs;
    private Dictionary<BuildingType, GameObject> buildingLookup;

    private Dictionary<Guid, Building> buildingInstances;

    private List<BuildingSpawn> recentActions;

    private struct BuildingSpawn
    {
        public Guid guid;
    }

    public string GetName() => "BuildingDatabase";
    public int GetPriority() => 1;

    private void Awake()
    {
        buildingInstances = new();
        buildingLookup = buildingPrefabs.Select(entry => (entry.GetComponent<Building>().Type, entry)).ToDictionary(keySelector: pair => pair.Type, elementSelector: pair => pair.entry);
        recentActions = new();

        StateTrack.Instance.AddProvider(this);
    }

    public string GetState()
    {
        recentActions.Clear();

        return JsonUtility.ToJson(new DatabaseState()
        {
            states = buildingInstances.Select(pair => new BuildingPair()
            {
                guid = pair.Key.ToString(),
                data = pair.Value.GetState(),
                type = pair.Value.Type,
                port = pair.Value.Port.Name
            }).ToList()
        });
    }

    public void SetState(string json)
    {
        var state = JsonUtility.FromJson<DatabaseState>(json);
        foreach (var building in buildingInstances.Values)
        {
            building.gameObject.SetActive(false);
            Destroy(building.gameObject);
        }
        buildingInstances.Clear();

        foreach (var pair in state.states)
        {
            var building = Spawn(pair.type, PortDatabase.Instance.Lookups[pair.port], Guid.Parse(pair.guid));
            building.SetState(pair.data);
        }

        recentActions.Clear();
    }


    public void Rollback()
    {
        for (int i = recentActions.Count - 1; i >= 0; i--)
        {
            var guid = recentActions[i].guid;
            var building = buildingInstances[guid];
            buildingInstances.Remove(guid);
            building.gameObject.SetActive(false);
            Destroy(building.gameObject);
        }

        foreach (var building in buildingInstances.Values)
            building.Rollback();
    }

    [ProButton]
    public Building Spawn(BuildingType type, Port port) => Spawn(type, port, Guid.NewGuid());
    private Building Spawn(BuildingType type, Port port, Guid guid)
    {
        var buidingObject = Instantiate(buildingLookup[type]);
        var building = buidingObject.GetComponent<Building>();
        building.Setup(port);

        buildingInstances.Add(guid, building);

        recentActions.Add(new BuildingSpawn()
        {
            guid = guid
        });

        return building;
    }
}