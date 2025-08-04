using UnityEngine;

public class NarrativePortVisual : MonoBehaviour
{
    [SerializeField] private Port port;

    public void Trigger()
    {
        port.Visual.enabled = true;
    }
}