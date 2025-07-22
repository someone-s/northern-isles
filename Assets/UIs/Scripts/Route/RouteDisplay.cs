using System.Collections.Generic;
using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(PortEvent))]
[RequireComponent(typeof(RouteCargo))]
public class RouteDisplay : MonoBehaviour
{
    public static RouteDisplay Instance { get; private set; }

    [SerializeField] private GameObject displayPrefab;
    [SerializeField] private RectTransform content;

    [SerializeField] private GameObject linePrefab;
    [SerializeField] private List<LineRenderer> lineRenderers;
    private Stack<LineRenderer> freeRenderers;
    private NavMeshPath path;
    private Vector3[] cornerBuffer;
    private const int MAX_ARRAY_SIZE = 64;

    public RouteCargo Cargo { get; private set; }
    public Vessel Vessel { get; private set; }
    public List<TMP_Dropdown.OptionData> CompartmentOptions { get; private set; }

    private PortEvent portEvent;

    public UnityEvent<Vessel> OnSelectedVessel;

    private RouteDisplay()
    {
        Instance = this;
    }

    private void Awake()
    {
        Cargo = GetComponent<RouteCargo>();

        portEvent = GetComponent<PortEvent>();
        portEvent.OnPortPressed.AddListener(OnPortPressed);

        lineRenderers = new();
        freeRenderers = new();
        path = new();
        cornerBuffer = new Vector3[MAX_ARRAY_SIZE];

        if (OnSelectedVessel == null)
            OnSelectedVessel = new();
    }

    public void LoadVessel(Vessel vessel)
    {
        Vessel = vessel;

        gameObject.SetActive(true);

        CompartmentOptions = Vessel.Compartments.Select(compartment => new TMP_Dropdown.OptionData(compartment.name)).ToList();

        foreach (var instruction in vessel.Instructions)
            AddInstruction(instruction);

        portEvent.PortDynamic();

        OnSelectedVessel.Invoke(vessel);
    }

    public void ExitRoute()
    {
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

        StatusDisplay.Instance.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }

    public void OnPortPressed(Port port)
    {
        if (Vessel == null) return;

        var instruction = Vessel.CreateInstruction(port.WayPoint);

        AddInstruction(instruction);
    }

    private void AddInstruction(VesselInstruction instruction)
    {
        var newUI = Instantiate(displayPrefab, content);
        newUI.transform.SetAsLastSibling();
        var data = newUI.GetComponent<RouteData>();

        data.SetDisplay(this);
        data.SetInstruction(instruction);

        CreateLineByPoint();
    }


    public void MoveInstruction(int oldIndex, int newIndex, VesselInstruction instruction)
    {
        Vessel.MoveInstruction(newIndex, instruction);

        UpdateLineByPoint(oldIndex, newIndex);
    }

    public bool DeleteInstruction(int index, VesselInstruction instruction)
    {
        bool success = Vessel.DeleteInstruction(instruction);
        if (success)
            RemoveLineByPoint(index);

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
        int instructionCount = Vessel.Instructions.Count;

        int wrapIndex;
        int fromIndex = (wrapIndex = index       % instructionCount) >= 0 ? wrapIndex : wrapIndex + instructionCount;
        int toIndex   = (wrapIndex = (index + 1) % instructionCount) >= 0 ? wrapIndex : wrapIndex + instructionCount;
        if (fromIndex >= lineRenderers.Count)
            return;

        var renderer = lineRenderers[fromIndex];

        var from = Vessel.Instructions[fromIndex];
        var to = Vessel.Instructions[toIndex];
        if (NavMesh.CalculatePath((
            from.wayPoint as IWaypoint).GetLocation(),
            (to.wayPoint as IWaypoint).GetLocation(),
            Vessel.Agent.areaMask, path))
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
}
