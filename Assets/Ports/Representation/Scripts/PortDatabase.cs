using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
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
        lookups.Add(port.Name, port);
    }

    public string GetName() => "PortDatabase";
    public int GetPriority() => 0;

    public JToken GetState()
    {
        return JToken.FromObject(new DatabaseState()
        {
            ports = ports.Select(port => new PortPair() { name = port.Name, data = port.GetState() }).ToList()
        });
    }

    public void SetState(JToken json)
    {
        var state = json.ToObject<DatabaseState>();
        foreach (var pair in state.ports)
            lookups[pair.name].SetState(pair.data);
    }

    public void Rollback()
    {
        foreach (var port in ports)
            port.Rollback();
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
        public JToken data;
    }
}