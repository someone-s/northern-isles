using TMPro;
using UnityEngine;

public class StatusData : MonoBehaviour
{
    public StatusDisplay Display { get; private set; }
    public Vessel Vessel { get; private set; }

    [SerializeField] private TMP_Text textArea;
    [SerializeField] private SpawnPanel spawnPanel;
    [SerializeField] private GameObject sectionPrefab;

    public void SetDisplay(StatusDisplay display)
    {
        Display = display;
    }

    public void SetVessel(Vessel vessel)
    {
        Vessel = vessel;

        textArea.text = Vessel.name;

        foreach (var compartment in Vessel.Compartments)
        {
            var sectionObject = spawnPanel.SpawnNewPanel(sectionPrefab);
            var section = sectionObject.GetComponent<StatusCompartment>();

            section.SetData(this);
            section.SetCompartment(compartment);
        }
    }

    public void LoadRoute()
    {
        Display.LoadRoute(Vessel);
    }
}