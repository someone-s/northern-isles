using UnityEngine;

public class NarrativeVesselInput : MonoBehaviour
{

    [SerializeField] private Vessel vessel;


    private void OnEnable()
    {
        if (vessel != null)
            vessel.Click.enabled = true;
    }

    private void OnDisable()
    {
        if (vessel != null)
            vessel.Click.enabled = false;
    }

    public void SetVessel(Vessel vessel)
    {
        this.vessel = vessel;
        this.vessel.Click.enabled = enabled;
    }
}