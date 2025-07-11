using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Courier : MonoBehaviour
{
    private NavMeshAgent agent;
    private const float serachRadius = 5f;
    private IBuilding home;
    private IBuilding destination;
    private Good good;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetHome(IBuilding building)
    {
        home = building;
        agent.Warp(building.GetLocation());
    }

    public void FindDestination(Good goodToDeliver)
    {
        good = goodToDeliver;

        Collider[] options = Physics.OverlapSphere(home.GetLocation(), serachRadius, good.type.GetLayerMask());
        if (options.Length < 1)
        {
            destination = null;
            home.ReturnHome(this);
            return;
        }

        Vector3 homeLocation = home.GetLocation();
        float minDistance = float.MaxValue;
        foreach (Collider collider in options)
        {
            IBuilding candidate = collider.GetComponentInParent<IBuilding>();
            float distance = Vector3.Distance(homeLocation, candidate.GetLocation());
            if (distance < minDistance)
            {
                minDistance = distance;
                destination = candidate;
            }
        }

        agent.SetDestination(destination.GetLocation());
        outbound = true;
    }


    private void DeliverItem()
    {
        destination.Recieve(good);
    }

    private bool outbound = false;
    private void Update()
    {
        if (outbound)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.01f)
            {
                DeliverItem();
                agent.SetDestination(home.GetLocation());
                outbound = false;
            }
        }
        else
        {

            if (!agent.pathPending && agent.remainingDistance < 0.01f)
            {
                home.ReturnHome(this);
            }
        }
            
        
    }
}