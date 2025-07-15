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
                loadActionData.SetData(loadAction);
            }
            else if (action is VesselUnloadAction)
            {
                var unloadAction = action as VesselUnloadAction;

                var unloadActionPanel = spawnPanel.SpawnNewPanel(unloadActionPrefab);
                var unloadActionData = unloadActionPanel.GetComponent<UnloadActionData>();
                unloadActionData.SetRoute(this);
                unloadActionData.SetData(unloadAction);
            }
        }
    }

    public void ChangeOrder(int index)
    {
        Display.ChangeInstructionIndex(index, Instruction);
    }

    public void AddLoad()
    {
        var action = Instruction.AddAction(typeof(VesselLoadAction)) as VesselLoadAction;

        var loadActionPanel = spawnPanel.SpawnNewPanel(loadActionPrefab);
        var loadActionData = loadActionPanel.GetComponent<LoadActionData>();
        loadActionData.SetRoute(this);
        loadActionData.SetData(action);
    }

    public void AddUnload()
    {
        var action = Instruction.AddAction(typeof(VesselUnloadAction)) as VesselUnloadAction;

        var unloadActionPanel = spawnPanel.SpawnNewPanel(unloadActionPrefab);
        var unloadActionData = unloadActionPanel.GetComponent<UnloadActionData>();
        unloadActionData.SetRoute(this);
        unloadActionData.SetData(action);
    }
}
