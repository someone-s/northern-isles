using UnityEngine;

[RequireComponent(typeof(NarrativeNode))]
public class RouteEdited : MonoBehaviour
{
    private NarrativeNode node;

    [SerializeField] private Vessel vessel;

    private void Awake()
    {
        node = GetComponent<NarrativeNode>();
    }

    private void Start()
    {
        RouteDisplay.Instance.OnVesselSelected.AddListener(Evaluate);
    }

    private void Evaluate(Vessel selectedVessel)
    {
        if (selectedVessel == vessel)
        {
            node.MarkComplete();
        }
    }
}
