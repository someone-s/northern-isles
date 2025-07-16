using UnityEngine;
using UnityEngine.AI;

public class VesselVisual : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform icon;
    private Vector3 previousLocation;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        agent.updateRotation = false;
    }

    public void Begin()
    {
        previousLocation = transform.position;
        enabled = true;
    }

    public void End()
    {
        enabled = false;
    }

    private void Update()
    {
        var currentLocation = transform.position;
        var delta = currentLocation - previousLocation;
        delta.y = 0f;

        if (delta.magnitude < 0.0001f)
            return;

        icon.rotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
    }
}