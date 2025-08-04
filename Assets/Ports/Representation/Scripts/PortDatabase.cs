using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class PortDatabase : MonoBehaviour, IStateProvider
{
    public static PortDatabase Instance { get; private set; }
    private PortDatabase()
    {
        Instance = this;

        ports ??= new();
        Ports ??= new(ports);
        lookups ??= new();
        Lookups ??= new(lookups);
    }

    private List<Port> ports;
    public ReadOnlyCollection<Port> Ports { get; private set; }
    private Dictionary<string, Port> lookups;
    public ReadOnlyDictionary<string, Port> Lookups { get; private set; }

    private void Awake()
    {
        StateTrack.Instance.AddProvider(this);
    }

    public void AddPort(Port port)
    {

        ports.Add(port);
        lookups.Add(port.Visual.Name, port);
    }

    public string GetName() => "PortDatabase";
    public int GetPriority() => 0;

    public string GetState()
    {
        return JsonUtility.ToJson(new DatabaseState()
        {
            ports = ports.Select(port => new PortPair() { name = port.Name, data = port.GetState() }).ToList()
        });
    }

    public void SetState(string json)
    {
        var state = JsonUtility.FromJson<DatabaseState>(json);
        foreach (var pair in state.ports)
            lookups[pair.name].SetState(pair.data);
    }

    public void Rollback()
    {
        throw new System.NotImplementedException();
    }

    [SerializeField]
    private struct DatabaseState
    {
        public List<PortPair> ports;
    }

    [SerializeField]
    private struct PortPair
    {
        public string name;
        public string data;
    }
}