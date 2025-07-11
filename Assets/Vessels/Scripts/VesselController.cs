using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VesselController : MonoBehaviour
{
    [SerializeField, InterfaceType(typeof(IWayPoint))] private List<Object> targets;
    [SerializeField] private int currentIndex = 0;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Refresh() => Change(currentIndex);
    private void Change(int newIndex)
    {
        // Dont move if no target
        if (targets.Count <= 0) return;

        // Make sure index within range
        currentIndex = newIndex % targets.Count;
        if (currentIndex < 0) currentIndex += targets.Count;

        agent.SetDestination((targets[currentIndex] as IWayPoint).GetLocation());
    }

    private void Update()
    {
        if (agent.remainingDistance < 0.01f)
            Change(currentIndex + 1);
    }
}
