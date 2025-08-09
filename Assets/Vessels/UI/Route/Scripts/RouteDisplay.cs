using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(PortEvent))]
public class RouteDisplay : MonoBehaviour
{
    public static RouteDisplay Instance { get; private set; }

    private RouteCompartment compartment;

    [SerializeField] private GameObject displayPrefab;
    [SerializeField] private RectTransform content;

    [SerializeField] private TMP_Text textArea;

    [SerializeField] private GameObject linePrefab;
    [SerializeField] private List<LineRenderer> lineRenderers;
    private Stack<LineRenderer> freeRenderers;
    private NavMeshPath path;
    private Vector3[] cornerBuffer;
    private const int MAX_ARRAY_SIZE = 64;

    public Vessel Vessel { get; private set; }

    private PortEvent portEvent;

    public UnityEvent<Vessel> OnVesselSelected;
    public Func<Port, bool> PortSelectOverride;
    public UnityEvent<RoutePort> OnPortAdded;
    public UnityEvent<RoutePort> OnPortMoved;
    public UnityEvent<RoutePort> OnPortDeleted;

    private RouteDisplay()
    {
        Instance = this;
    }

    private void Awake()
    {
        portEvent = GetComponent<PortEvent>();
        portEvent.OnPortPressed.AddListener(OnPortPressed);

        lineRenderers = new();
        freeRenderers = new();
        path = new();
        cornerBuffer = new Vector3[MAX_ARRAY_SIZE];

        if (OnVesselSelected == null)
            OnVesselSelected = new();
        if (OnPortAdded == null)
            OnPortAdded = new();
        if (OnPortMoved == null)
            OnPortMoved = new();
        if (OnPortDeleted == null)
            OnPortDeleted = new();

        compartment = GetComponentInChildren<RouteCompartment>();
        compartment.SetDisplay(this);

        StateTrack.Instance.OnBeginLoadState.AddListener(ExitRouteChecked);
        StateTrack.Instance.OnBeginRollback.AddListener(ExitRouteChecked);
    }

    public void LoadVessel(Vessel vessel)
    {
        Vessel = vessel;

        gameObject.SetActive(true);

        foreach (var port in vessel.Navigation.Ports)
            AddPort(port);

        portEvent.PortDynamic();

        compartment.SetStorage(Vessel.Storage);

        OnVesselSelected.Invoke(vessel);

        textArea.text = Vessel.name;
    }

    private void ExitRouteChecked()
    {
        if (gameObject.activeSelf)
            ExitRoute();
    }
    public void ExitRoute()
    {
        compartment.RemoveStorage(Vessel.Storage);
        
        portEvent.PortStatic();

        for (int i = content.childCount - 1; i >= 0; i--)
        {
            var t = content.GetChild(i);
            Destroy(t.gameObject);
        }
        content.DetachChildren();

        if (content.TryGetComponent<RefreshablePanel>(out var panel))
            panel.Refresh();

        Vessel = null;

        for (int i = lineRenderers.Count - 1; i >= 0; i--)
        {
            var renderer = lineRenderers[i];
            renderer.gameObject.SetActive(false);
            lineRenderers.RemoveAt(i);
            freeRenderers.Push(renderer);
        }

        gameObject.SetActive(false);
    }

    public void OnPortPressed(Port port)
    {
        if (Vessel == null) return;

        if (PortSelectOverride != null && !PortSelectOverride.Invoke(port)) return;

        Vessel.Navigation.AddPort(port);

        RoutePort data = AddPort(port);

        OnPortAdded.Invoke(data);
    }

    private RoutePort AddPort(Port port)
    {
        var newUI = Instantiate(displayPrefab, content);
        newUI.transform.SetAsLastSibling();
        var data = newUI.GetComponent<RoutePort>();

        data.SetDisplay(this);
        data.SetPort(port);

        CreateLineByPoint();

        return data;
    }


    public void MovePort(RoutePort data, int oldIndex, int newIndex, Port port)
    {
        Vessel.Navigation.MovePort(newIndex, port);

        UpdateLineByPoint(oldIndex, newIndex);

        OnPortMoved.Invoke(data);
    }

    public bool DeletePort(RoutePort data, int index, Port port)
    {
        bool success = Vessel.Navigation.DeletePort(port);
        if (success)
        {
            RemoveLineByPoint(index);
            OnPortDeleted.Invoke(data);
        }

        return success;

    }


    private void CreateLineByPoint()
    {
        CreateLine();
        UpdateLineByPoint(lineRenderers.Count - 1);
    }
    private void CreateLine()
    {
        LineRenderer renderer;
        if (freeRenderers.TryPop(out renderer))
            renderer.gameObject.SetActive(true);
        else
        {
            var newLine = Instantiate(linePrefab);
            renderer = newLine.GetComponent<LineRenderer>();
        }
        lineRenderers.Add(renderer);
    }

    private void UpdateLineByPoint(int oldIndex, int newIndex)
    {
        UpdateLineByPoint(oldIndex);
        UpdateLineByPoint(newIndex);
    }
    private void UpdateLineByPoint(int index)
    {
        UpdateLine(index - 1);
        UpdateLine(index);
    }
    private void UpdateLine(int index)
    {
        int instructionCount = Vessel.Navigation.Ports.Count;

        int wrapIndex;
        int fromIndex = (wrapIndex = index % instructionCount) >= 0 ? wrapIndex : wrapIndex + instructionCount;
        int toIndex = (wrapIndex = (index + 1) % instructionCount) >= 0 ? wrapIndex : wrapIndex + instructionCount;
        if (fromIndex >= lineRenderers.Count)
            return;

        var renderer = lineRenderers[fromIndex];

        var from = Vessel.Navigation.Ports[fromIndex];
        var to = Vessel.Navigation.Ports[toIndex];
        if (NavMesh.CalculatePath(
            from.WayPoint.GetLocation(),
            to.WayPoint.GetLocation(),
            Vessel.Navigation.Agent.areaMask, path))
        {
            int count = path.GetCornersNonAlloc(cornerBuffer);
            renderer.positionCount = count;
            renderer.SetPositions(cornerBuffer);
        }
    }

    private void RemoveLineByPoint(int index)
    {
        var renderer = lineRenderers[index];
        lineRenderers.RemoveAt(index);
        renderer.gameObject.SetActive(false);
        freeRenderers.Push(renderer);


        UpdateLineByPoint(index);
    }

    public void DeleteLoad(CargoType cargo)
    {
        Vessel.Storage.Remove(cargo);
    }
}
