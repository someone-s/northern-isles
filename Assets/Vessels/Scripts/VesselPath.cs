using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VesselPath : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject linePrefab;
    private LineRenderer lineRenderer;
    private const int MAX_ARRAY_SIZE = 64;
    private Vector3[] array;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        array = new Vector3[MAX_ARRAY_SIZE];
        var lineVisual = Instantiate(linePrefab);
        lineRenderer = lineVisual.GetComponent<LineRenderer>();
    }

    public void Begin()
    {
        enabled = true;
    }

    public void End()
    {
        enabled = false;
    }


    private void Update()
    {
        if (!agent.pathPending)
        {
            lineRenderer.positionCount = agent.path.GetCornersNonAlloc(array);
            lineRenderer.SetPositions(array);
        }
    }
}
