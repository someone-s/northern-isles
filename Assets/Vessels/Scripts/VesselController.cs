using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class VesselController : MonoBehaviour
{
    [SerializeField, InterfaceType(typeof(IWayPoint))] private List<Object> targets;
    [SerializeField] private int currentIndex = 0;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Assert.IsNotNull(agent);
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
