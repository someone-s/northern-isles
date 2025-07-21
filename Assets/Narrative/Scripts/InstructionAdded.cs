using AYellowpaper;
using UnityEngine;

[RequireComponent(typeof(NarrativeNode))]
public class NarrativeInstruction : MonoBehaviour
{
    private NarrativeNode node;
    [SerializeField] private Vessel vessel;

    private void Awake()
    {
        node = GetComponent<NarrativeNode>();
        vessel.OnCreateInstruction.AddListener(OnCreateInstruction);
    }

    [RequireInterface(typeof(IWaypoint))]
    public Object expectedWayPoint;

    public void OnCreateInstruction(VesselInstruction instruction)
    {
        if (instruction.wayPoint == expectedWayPoint)
            node.MarkComplete();
    }
}
