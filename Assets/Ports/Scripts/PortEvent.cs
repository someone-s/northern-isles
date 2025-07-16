using UnityEngine;
using UnityEngine.Events;


public class PortEvent : MonoBehaviour
{
    public static PortEvent Instance { get; private set; }

    public UnityEvent<Port> OnPortPressed;

    private PortEvent()
    {
        Instance = this;
    }

    public void PortPressed(Port port)
    {
        OnPortPressed.Invoke(port);
    }
}