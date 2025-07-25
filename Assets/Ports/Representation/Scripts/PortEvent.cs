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

    public void PortDynamic()
    {
        foreach (var port in Port.Ports)
            port.Visual.SetIconMode(PortVisual.IconMode.Dynamic);
    }

    public void PortStatic()
    {
        foreach (var port in Port.Ports)
            port.Visual.SetIconMode(PortVisual.IconMode.Static);
    }

    public void PortPressed(Port port)
    {
        OnPortPressed.Invoke(port);
    }
}