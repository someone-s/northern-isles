using UnityEngine;
using UnityEngine.Events;

public class NarrativeInstructionAdd : MonoBehaviour
{
    [SerializeField] private Port expectedPort;

    public UnityEvent OnCorrectPortSelected;
    public UnityEvent OnWrongPortSelected;

    private void OnEnable()
    {
        RouteDisplay.Instance.PortSelectOverride = FilterPort;
        RouteDisplay.Instance.OnInstructionAdded.AddListener(InstructionAdded);
    }

    private void OnDisable()
    {
        UnFilter();
        RouteDisplay.Instance.OnInstructionAdded.RemoveListener(InstructionAdded);
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
            RouteDisplay.Instance.OnInstructionAdded.RemoveListener(InstructionAdded);
            RouteDisplay.Instance.PortSelectOverride = AlwaysFalse;
        }
    }
}
