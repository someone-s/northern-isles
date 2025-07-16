using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VesselPath : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject linePrefab;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        var lineVisual = Instantiate(linePrefab);
        lineRenderer = lineVisual.GetComponent<LineRenderer>();
    }

    private bool needNewPath = false;
    public void Begin()
    {
        needNewPath = true;
        enabled = true;
    }

    public void End()
    {
        enabled = false;
    }

    private void TryGetNewPath()
    {
        if (!agent.pathPending)
        {
            var corners = agent.path.corners;
            lineRenderer.positionCount = corners.Length;

            for (int i = 0; i < corners.Length; i++)
                lineRenderer.SetPosition(i, corners[corners.Length - 1 - i]);

            float totalLength = 0f;
            for (int i = 1; i < corners.Length; i++)
                totalLength += Vector3.Distance(corners[i - 1], corners[i]);

            progress = 0f;
            previousPosition = transform.position;

            lineRenderer.material.SetFloat("_Length", totalLength);
            lineRenderer.material.SetFloat("_Progress", progress);


            needNewPath = false;
        }
    }

    private float progress;
    private Vector3 previousPosition;

    private void Update()
    {
        if (needNewPath)
            TryGetNewPath();
        else
        {
            var currentPosition = transform.position;
            progress += Vector3.Distance(previousPosition, currentPosition);
            previousPosition = currentPosition;

            lineRenderer.material.SetFloat("_Progress", progress);
        }
    }
}
