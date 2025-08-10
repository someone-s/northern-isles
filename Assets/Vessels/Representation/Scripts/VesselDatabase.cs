using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class VesselDatabase : MonoBehaviour, IStateProvider
{
    public static VesselDatabase Instance { get; private set; }

    [Serializable]
    private struct DatabaseState
    {
        public List<VesselPair> states;
    }

    [Serializable]
    private struct VesselPair
    {
        public string guid;
        public string name;
        public string type;
        public JToken data;
    }

    [SerializeField] private List<GameObject> vesselPrefabs;
    private Dictionary<string, GameObject> vesselLookups;

    private Dictionary<Guid, Vessel> vesselInstances;
    public ReadOnlyDictionary<Guid, Vessel> VesselInstances;

    private List<VesselSpawn> recentActions;

    private struct VesselSpawn
    {
        public Guid guid;
    }

    public string GetName() => "VesselDatabase";
    public int GetPriority() => 2;

    private VesselDatabase()
    {
        Instance = this;
    }

    private void Awake()
    {
        vesselInstances = new();
        VesselInstances = new(vesselInstances);
        vesselLookups = vesselPrefabs.Select(entry => (entry.GetComponent<Vessel>().Type, entry)).ToDictionary(keySelector: pair => pair.Type, elementSelector: pair => pair.entry);
        recentActions = new();

        StateTrack.Instance.AddProvider(this);
    }

    public JToken GetState()
    {
        recentActions.Clear();

        return JToken.FromObject(new DatabaseState()
        {
            states = vesselInstances.Select(pair => new VesselPair()
            {
                guid = pair.Key.ToString(),
                type = pair.Value.Type,
                name = pair.Value.name,
                data = pair.Value.GetState()
            }).ToList()
        });
    }

    public void SetState(JToken json)
    {
        var state = json.ToObject<DatabaseState>();
        foreach (var vessel in vesselInstances.Values)
        {
            vessel.gameObject.SetActive(false);
            Destroy(vessel.gameObject);
        }
        vesselInstances.Clear();

        foreach (var pair in state.states)
        {
            var vessel = Spawn(pair.type, pair.name, Guid.Parse(pair.guid));
            vessel.SetState(pair.data);
        }

        recentActions.Clear();
    }


    public void Rollback()
    {
        for (int i = recentActions.Count - 1; i >= 0; i--)
        {
            var guid = recentActions[i].guid;
            var vessel = vesselInstances[guid];
            vesselInstances.Remove(guid);
            vessel.gameObject.SetActive(false);
            Destroy(vessel.gameObject);
        }

        foreach (var vessel in vesselInstances.Values)
            vessel.Rollback();
    }

    public void Spawn(VesselSpawnSetting settings)
    {
        var vessel = Spawn(settings.type, settings.vesselName, settings.orientation);
        settings.OnVesselSpawn.Invoke(vessel);
    }
    [Button()]
    public Vessel Spawn(string type, string name, Transform orientation)
    {
        var vessel = Spawn(type, name, Guid.NewGuid());
        vessel.SetOrientation(orientation.position, orientation.rotation);
        return vessel;
    }
    private Vessel Spawn(string type, string name, Guid guid)
    {
        var vesselObject = Instantiate(vesselLookups[type], transform);
        var vessel = vesselObject.GetComponent<Vessel>();
        vessel.name = name;
        vessel.SetGuid(guid);

        vesselInstances.Add(guid, vessel);

        recentActions.Add(new VesselSpawn()
        {
            guid = guid
        });

        return vessel;
    }
}