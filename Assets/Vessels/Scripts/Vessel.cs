using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Vessel : MonoBehaviour
{
    public Account Account { get; private set; }

    [SerializeField] private List<VesselCompartment> compartments;
    public ReadOnlyCollection<VesselCompartment> Compartments;

    [SerializeField] private List<VesselInstruction> instructions;
    public ReadOnlyCollection<VesselInstruction> Instructions;
    [SerializeField] private int currentIndex = 0;

    private NavMeshAgent agent;
    private bool lost;

    public UnityEvent OnChangeDestination;
    public UnityEvent OnReachedDestination;
    public UnityEvent<float> OnTransaction;

    public VesselInstruction CreateInstruction(IWaypoint waypoint)
    {
        if (waypoint is not Object)
            return null;

        var instruction = gameObject.AddComponent<VesselInstruction>();
        instruction.wayPoint = waypoint as Object;
        instructions.Add(instruction);
        return instruction;
    }

    public void MoveInstruction(int index, VesselInstruction instruction)
    {
        instructions.Remove(instruction);
        instructions.Insert(index, instruction);
    }

    public void DeleteInstruction(VesselInstruction instruction)
    {
        instructions.Remove(instruction);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Compartments = new ReadOnlyCollection<VesselCompartment>(compartments);
        Instructions = new(instructions);
        Account = GetComponent<Account>();
    }

    private void Refresh() => Change(currentIndex);
    private void Change(int newIndex)
    {
        // Dont move if no target
        if (instructions.Count <= 0)
        {
            lost = true;
            return;
        }

        // Make sure index within range
            currentIndex = newIndex % instructions.Count;
        if (currentIndex < 0) currentIndex += instructions.Count;

        agent.SetDestination((instructions[currentIndex].wayPoint as IWaypoint).GetLocation());

        OnChangeDestination.Invoke();

        lost = false;
    }

    private void Update()
    {
        if (lost)
        {
            Change(currentIndex);
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.01f)
        {
            OnReachedDestination.Invoke();

            if (instructions.Count > 0)
            {
                var instruction = instructions[currentIndex];
                foreach (var action in instruction.actions)
                    (action as IVesselAction).PerformAction(this, instruction.wayPoint as IWaypoint);
            }

            Change(currentIndex + 1);
        }
    }
}
