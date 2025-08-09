using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class BuildingDatabase : MonoBehaviour, IStateProvider
{
    public static BuildingDatabase Instance { get; private set; }


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
        public int position;
        public JToken data;
    }


    [SerializeField] private List<GameObject> buildingPrefabs;
    private Dictionary<BuildingType, GameObject> buildingLookup;

    private Dictionary<Guid, Building> buildingInstances;
    private Dictionary<Port, List<Building>> buildingNeighbours;
    public ReadOnlyDictionary<Port, List<Building>> BuildingNeighbours;

    private List<BuildingSpawn> recentActions;

    private struct BuildingSpawn
    {
        public Guid guid;
    }

    public string GetName() => "BuildingDatabase";
    public int GetPriority() => 1;

    private BuildingDatabase()
    {
        Instance = this;
    }

    private void Awake()
    {
        buildingInstances = new();
        buildingNeighbours = new();
        BuildingNeighbours = new(buildingNeighbours);
        buildingLookup = buildingPrefabs.Select(entry => (entry.GetComponent<Building>().Type, entry)).ToDictionary(keySelector: pair => pair.Type, elementSelector: pair => pair.entry);
        recentActions = new();

        StateTrack.Instance.AddProvider(this);
    }

    public JToken GetState()
    {
        recentActions.Clear();

        return JToken.FromObject(new DatabaseState()
        {
            states = buildingInstances.Select(pair => new BuildingPair()
            {
                guid = pair.Key.ToString(),
                type = pair.Value.Type,
                port = pair.Value.Port.Name,
                position = pair.Value.Position,
                data = pair.Value.GetState()
            }).ToList()
        });
    }

    public void SetState(JToken json)
    {
        var state = json.ToObject<DatabaseState>();
        foreach (var building in buildingInstances.Values)
        {
            building.gameObject.SetActive(false);
            Destroy(building.gameObject);
        }
        buildingInstances.Clear();
        buildingNeighbours.Clear();

        foreach (var pair in state.states)
        {
            var building = Spawn(pair.type, PortDatabase.Instance.Lookups[pair.port], pair.position, Guid.Parse(pair.guid));
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
            buildingNeighbours[building.Port].Remove(building);

            building.gameObject.SetActive(false);
            Destroy(building.gameObject);
        }

        foreach (var building in buildingInstances.Values)
            building.Rollback();
    }

    public void Spawn(BuildingSpawnSetting settings) => Spawn(settings.type, settings.port, settings.position); 
    [Button()]
    public Building Spawn(BuildingType type, Port port, int position) => Spawn(type, port, position, Guid.NewGuid());
    private Building Spawn(BuildingType type, Port port, int position, Guid guid)
    {
        var buidingObject = Instantiate(buildingLookup[type], transform);
        var building = buidingObject.GetComponent<Building>();

        buildingInstances.Add(guid, building);
        List<Building> neighbours;
        if (!buildingNeighbours.TryGetValue(port, out neighbours))
        {
            neighbours = new();
            buildingNeighbours.Add(port, neighbours);
        }
        neighbours.Add(building);

        building.Setup(port, guid, position);

        recentActions.Add(new BuildingSpawn()
        {
            guid = guid
        });

        return building;
    }
}