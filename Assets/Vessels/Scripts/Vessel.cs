using System.Collections.Generic;
using System.Collections.ObjectModel;
using AYellowpaper;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Vessel : MonoBehaviour
{
    public VesselCompartment[] Compartments { get; private set; }

    [SerializeField] private List<VesselInstruction> instructions;
    public ReadOnlyCollection<VesselInstruction> Instructions;
    [SerializeField] private int currentIndex = 0;

    private NavMeshAgent agent;

    public UnityEvent OnChangeDestination;

    public void MoveInstruction(int index, VesselInstruction instruction)
    {
        instructions.Remove(instruction);
        instructions.Insert(index, instruction);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Compartments = GetComponentsInChildren<VesselCompartment>();
        Instructions = new(instructions);
    }

    private void Refresh() => Change(currentIndex);
    private void Change(int newIndex)
    {
        // Dont move if no target
        if (instructions.Count <= 0) return;

        // Make sure index within range
        currentIndex = newIndex % instructions.Count;
        if (currentIndex < 0) currentIndex += instructions.Count;

        agent.SetDestination((instructions[currentIndex].wayPoint as IWaypoint).GetLocation());
        
        OnChangeDestination.Invoke();
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.01f)
        {
            var instruction = instructions[currentIndex];
            foreach (var action in instruction.actions)
                (action as IVesselAction).PerformAction(this, instruction.wayPoint as IWaypoint);
            Change(currentIndex + 1);
        }
    }
}
