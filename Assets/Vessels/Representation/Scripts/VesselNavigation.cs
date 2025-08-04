using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class VesselNavigation : MonoBehaviour
{
    public Vessel Vessel { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Vessel = GetComponentInParent<Vessel>();

        ports ??= new();
        Ports = new(ports);

        GetState();
    }

    public ReadOnlyCollection<Port> Ports;
    [SerializeField] private List<Port> ports;
    [SerializeField] private int currentIndex = 0;
    private bool lost;

    private JToken cachedState;

    public UnityEvent OnChangeDestination;
    public UnityEvent OnReachedDestination;
    public UnityEvent<Port> OnCreateWaypoint;

    public void AddPort(Port port)
    {
        ports.Add(port);

        OnCreateWaypoint.Invoke(port);
    }

    public void MovePort(int targetIndex, Port port)
    {
        int originalIndex = ports.IndexOf(port);

        if (originalIndex == targetIndex)
            return;


        ports.Remove(port);
        ports.Insert(targetIndex, port);

        if (originalIndex < targetIndex)
        {
            if (currentIndex > originalIndex && currentIndex <= targetIndex)
                currentIndex -= 1;
            else if (currentIndex == originalIndex)
                currentIndex = targetIndex;
        }
        else // targetindex < originalIndex
        {
            if (currentIndex >= targetIndex && currentIndex < originalIndex)
                currentIndex += 1;
            else if (currentIndex == originalIndex)
                currentIndex = targetIndex;
        }

        Refresh();
    }

    public bool DeletePort(Port port)
    {
        if (ports.Count <= 1)
            return false;

        ports.Remove(port);

        int originalIndex = ports.IndexOf(port);
        if (currentIndex == originalIndex)
        {
            currentIndex += 1;
            currentIndex %= ports.Count;
        }
        Refresh();

        return true;
    }

    private void Refresh() => Change(currentIndex);
    private void Change(int newIndex)
    {
        // Dont move if no target
        if (ports.Count <= 0)
        {
            lost = true;
            return;
        }

        // Make sure index within range
        currentIndex = newIndex % ports.Count;
        if (currentIndex < 0) currentIndex += ports.Count;

        Agent.SetDestination(ports[currentIndex].WayPoint.GetLocation());

        OnChangeDestination.Invoke();

        lost = false;
    }

    private void Update()
    {
        if (lost)
        {
            Change(currentIndex);
        }
        else if (!Agent.pathPending && Agent.remainingDistance < 0.01f)
        {
            OnReachedDestination.Invoke();

            if (currentIndex < ports.Count && currentIndex >= 0 && ports.Count >= 2)
            {
                Vessel.Storage.Unload(ports[currentIndex]);
                Vessel.Storage.Load(ports[currentIndex]);
            }

            Change(currentIndex + 1);
        }
    }

    public JToken GetState()
    {
        cachedState = JToken.FromObject(new NavigationState()
        {
            ports = ports.Select(port => port.Name).ToList(),
            currentIndex = currentIndex,
            lost = lost
        });
        return cachedState;
    }

    public void SetState(JToken json)
    {
        cachedState = json;

        var state = cachedState.ToObject<NavigationState>();
        ports = state.ports.Select(name => PortDatabase.Instance.Lookups[name]).ToList();
        Ports = new(ports);
        currentIndex = state.currentIndex;
        lost = state.lost;
    }

    public void Rollback()
    {
        SetState(cachedState);
    }

    private struct NavigationState
    {
        public List<string> ports;
        public int currentIndex;
        public bool lost;
    }
}
