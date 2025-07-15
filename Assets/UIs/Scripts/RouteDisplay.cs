using System.Collections.Generic;
using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RouteCargo))]
public class RouteDisplay : MonoBehaviour
{
    [SerializeField] private GameObject portWaypointPrefab;
    [SerializeField] private RectTransform content;

    public RouteCargo Cargo { get; private set; }

    public Vessel Vessel { get; private set; }
    public List<TMP_Dropdown.OptionData> CompartmentOptions { get; private set; }


    private void Awake()
    {
        Cargo = GetComponent<RouteCargo>();
    }

    public void MoveInstruction(int index, VesselInstruction instruction)
    {
        Vessel.MoveInstruction(index, instruction);
    }

    [ProButton]
    public void LoadVessel(Vessel vessel)
    {
        Vessel = vessel;

        CompartmentOptions = Vessel.Compartments.Select(compartment => new TMP_Dropdown.OptionData(compartment.name)).ToList();

        foreach (var route in content)
        {
            Destroy((route as Transform).gameObject);
        }

        foreach (var instruction in vessel.Instructions)
        {
            if (instruction.wayPoint is PortWaypoint)
            {
                GameObject newUI = Instantiate(portWaypointPrefab, content);
                newUI.transform.SetAsLastSibling();
                RouteData data = newUI.GetComponent<RouteData>();

                data.SetDisplay(this);
                data.SetInstruction(instruction);
            }

        }
    }
}
