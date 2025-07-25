using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RouteData : MonoBehaviour
{

    [SerializeField] private TMP_Text textArea;
    [SerializeField] private SpawnPanel spawnPanel;
    [SerializeField] private GameObject loadActionPrefab;
    [SerializeField] private GameObject unloadActionPrefab;

    public VesselInstruction Instruction { get; private set; }
    public RouteDisplay Display { get; private set; }
    public string Name { get; private set; }

    public Func<bool> LoadAddOverride = null;
    public UnityEvent<LoadActionData> OnLoadAdded;
    public Func<bool> UnloadAddOverride = null;
    public UnityEvent<UnloadActionData> OnUnloadAdded;
    public UnityEvent<List<IVesselAction>> OnActionMoved;

    private void Awake()
    {
        if (OnLoadAdded == null)
            OnLoadAdded = new();
        if (OnUnloadAdded == null)
            OnUnloadAdded = new();
        if (OnActionMoved == null)
            OnActionMoved = new();
    }

    public void SetDisplay(RouteDisplay display)
    {
        Display = display;
    }

    public void SetInstruction(VesselInstruction instruction)
    {
        Instruction = instruction;

        var portName = (instruction.wayPoint as PortWaypoint).Port.Visual.Name;
        textArea.text = portName;
        Name = portName;

        IEnumerable<IVesselAction> actions = instruction.actions.Where(action => action is IVesselAction).Select(action => action as IVesselAction);
    
        foreach (var action in actions)
        {

            if (action is VesselLoadAction)
            {
                var loadAction = action as VesselLoadAction;

                var loadActionPanel = spawnPanel.SpawnNewPanel(loadActionPrefab);
                var loadActionData = loadActionPanel.GetComponent<LoadActionData>();
                loadActionData.SetRoute(this);
                loadActionData.SetAction(loadAction);
            }
            else if (action is VesselUnloadAction)
            {
                var unloadAction = action as VesselUnloadAction;

                var unloadActionPanel = spawnPanel.SpawnNewPanel(unloadActionPrefab);
                var unloadActionData = unloadActionPanel.GetComponent<UnloadActionData>();
                unloadActionData.SetRoute(this);
                unloadActionData.SetAction(unloadAction);
            }
        }
    }

    public void MoveInstruction(int oldIndex, int newIndex)
    {  
        
        Display.MoveInstruction(oldIndex, newIndex, Instruction);
        
    }

    public void DeleteInstruction()
    {
        bool success = Display.DeleteInstruction(transform.GetSiblingIndex(), Instruction);
        if (!success)
            return;

        var deletablePanel = GetComponent<DeletablePanel>();
        if (deletablePanel != null)
            deletablePanel.Delete();
            

    }

    public void MoveAction(int index, IVesselAction action)
    {
        Instruction.MoveAction(index, action);

        OnActionMoved.Invoke(Instruction.actions);
    }

    public void DeleteAction(IVesselAction action)
    {
        Instruction.DeleteAction(action);
    }

    public void AddLoad()
    {
        if (LoadAddOverride != null && !LoadAddOverride.Invoke()) return;

        var action = Instruction.AddAction(typeof(VesselLoadAction)) as VesselLoadAction;

        var loadActionPanel = spawnPanel.SpawnNewPanel(loadActionPrefab);
        var loadActionData = loadActionPanel.GetComponent<LoadActionData>();
        loadActionData.SetRoute(this);
        loadActionData.SetAction(action);

        OnLoadAdded.Invoke(loadActionData);
    }

    public void AddUnload()
    {
        if (UnloadAddOverride != null && !UnloadAddOverride.Invoke()) return;

        var action = Instruction.AddAction(typeof(VesselUnloadAction)) as VesselUnloadAction;

        var unloadActionPanel = spawnPanel.SpawnNewPanel(unloadActionPrefab);
        var unloadActionData = unloadActionPanel.GetComponent<UnloadActionData>();
        unloadActionData.SetRoute(this);
        unloadActionData.SetAction(action);

        OnUnloadAdded.Invoke(unloadActionData);
    }
}
