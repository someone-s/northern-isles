using AYellowpaper;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NarrativeNode))]
public class InstructionAddStep : MonoBehaviour
{
    private NarrativeNode node;
    [SerializeField] private Port expectedPort;

    public UnityEvent OnCorrectPortSelected;
    public UnityEvent OnWrongPortSelected;
    public UnityEvent<RouteData> OnInstructionAdded;


    private void Awake()
    {
        node = GetComponent<NarrativeNode>();
    }

    private void Start()
    {
        RouteDisplay.Instance.PortSelectOverride = FilterPort;
        RouteDisplay.Instance.OnInstructionAdded.AddListener(InstructionAdded);
    }

    public void UnFilter()
    {

        if (RouteDisplay.Instance.PortSelectOverride == FilterPort)
            RouteDisplay.Instance.PortSelectOverride = null;
        if (RouteDisplay.Instance.PortSelectOverride == AlwaysFalse)
            RouteDisplay.Instance.PortSelectOverride = null;
    }

    private bool acceptIncomingInstruction = false;

    private bool FilterPort(Port port)
    {
        if (port == expectedPort)
        {
            OnCorrectPortSelected.Invoke();
            acceptIncomingInstruction = true;
            return true;
        }
        else
        {
            OnWrongPortSelected.Invoke();
            return false;
        }
    }

    private bool AlwaysFalse(Port _) => false;

    public void InstructionAdded(RouteData instructionData)
    {
        if (acceptIncomingInstruction)
        {
            OnInstructionAdded.Invoke(instructionData);
            RouteDisplay.Instance.OnInstructionAdded.RemoveListener(InstructionAdded);
            RouteDisplay.Instance.PortSelectOverride = AlwaysFalse;
            node.MarkComplete();
        }
    }
}
