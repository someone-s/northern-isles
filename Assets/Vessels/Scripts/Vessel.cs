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

    public NavMeshAgent Agent { get; private set; }
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

    public void MoveInstruction(int targetIndex, VesselInstruction instruction)
    {
        int originalIndex = instructions.IndexOf(instruction);

        if (originalIndex == targetIndex)
            return;


        instructions.Remove(instruction);
        instructions.Insert(targetIndex, instruction);

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

    public bool DeleteInstruction(VesselInstruction instruction)
    {
        if (instructions.Count <= 1)
            return false;

        instructions.Remove(instruction);

        int originalIndex = instructions.IndexOf(instruction);
        if (currentIndex == originalIndex)
        {
            currentIndex += 1;
            currentIndex %= instructions.Count;
        }
        Refresh();

        return true;
    }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
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

        Agent.SetDestination((instructions[currentIndex].wayPoint as IWaypoint).GetLocation());

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

            if (currentIndex < instructions.Count && currentIndex >= 0)
            {
                var instruction = instructions[currentIndex];
                foreach (var action in instruction.actions)
                    (action as IVesselAction).PerformAction(this, instruction.wayPoint as IWaypoint);
            }

            Change(currentIndex + 1);
        }
    }
}
