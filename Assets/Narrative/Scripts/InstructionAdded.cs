using AYellowpaper;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NarrativeNode))]
public class NarrativeInstruction : MonoBehaviour
{
    private NarrativeNode node;
    [SerializeField] private Vessel vessel;

    public UnityEvent<VesselInstruction> OnCreateInstruction;

    private void Awake()
    {
        node = GetComponent<NarrativeNode>();
        vessel.OnCreateInstruction.AddListener(OnCreatedInstruction);
    }

    [RequireInterface(typeof(IWaypoint))]
    public MonoBehaviour expectedWayPoint;

    public void OnCreatedInstruction(VesselInstruction instruction)
    {
        if (instruction.wayPoint == expectedWayPoint)
            node.MarkComplete();

        OnCreateInstruction.Invoke(instruction);
    }
}
