using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RouteData : MonoBehaviour
{

    [SerializeField] private TMP_Text textArea;
    [SerializeField] private SpawnPanel spawnPanel;
    [SerializeField] private GameObject loadActionPrefab;
    [SerializeField] private GameObject unloadActionPrefab;

    public VesselInstruction Instruction { get; private set; }
    public RouteDisplay Display { get; private set; }
    public string Name { get; private set; }

    public void SetDisplay(RouteDisplay display)
    {
        Display = display;
    }

    public void SetInstruction(VesselInstruction instruction)
    {
        Instruction = instruction;

        var portName = (instruction.wayPoint as PortWaypoint).Port.name;
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

    public void ChangeInstructionOrder(int index)
    {
        Display.MoveInstruction(index, Instruction);
    }

    public void MoveAction(int index, IVesselAction action)
    {
        Instruction.MoveAction(index, action);
    }

    public void AddLoad()
    {
        var action = Instruction.AddAction(typeof(VesselLoadAction)) as VesselLoadAction;

        var loadActionPanel = spawnPanel.SpawnNewPanel(loadActionPrefab);
        var loadActionData = loadActionPanel.GetComponent<LoadActionData>();
        loadActionData.SetRoute(this);
        loadActionData.SetAction(action);
    }

    public void AddUnload()
    {
        var action = Instruction.AddAction(typeof(VesselUnloadAction)) as VesselUnloadAction;

        var unloadActionPanel = spawnPanel.SpawnNewPanel(unloadActionPrefab);
        var unloadActionData = unloadActionPanel.GetComponent<UnloadActionData>();
        unloadActionData.SetRoute(this);
        unloadActionData.SetAction(action);
    }
}
