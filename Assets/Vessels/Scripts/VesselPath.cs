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

    public void Refresh()
    {
        enabled = true;
    }

    private void Update()
    {
        if (!agent.pathPending)
        {
            var corners = agent.path.corners;
            lineRenderer.positionCount = corners.Length;
            lineRenderer.SetPositions(corners.Reverse().ToArray());
            lineRenderer.material.mainTextureScale = new(1f / lineRenderer.startWidth, 1f);

            enabled = false;

        }
    }
}
