using System.Collections.Generic;
using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(RouteCargo))]
public class RouteDisplay : MonoBehaviour
{
    public static RouteDisplay Instance { get; private set; }

    [SerializeField] private GameObject displayPrefab;
    [SerializeField] private RectTransform content;

    public RouteCargo Cargo { get; private set; }

    public Vessel Vessel { get; private set; }
    public List<TMP_Dropdown.OptionData> CompartmentOptions { get; private set; }

    private RouteDisplay()
    {
        Instance = this;
    }

    private void Awake()
    {
        Cargo = GetComponent<RouteCargo>();
    }

    public void MoveInstruction(int index, VesselInstruction instruction)
    {
        Vessel.MoveInstruction(index, instruction);
    }

    public void DeleteInstruction(VesselInstruction instruction)
    {
        Vessel.DeleteInstruction(instruction);
    }

    [ProButton]
    public void LoadVessel(Vessel vessel)
    {
        Vessel = vessel;

        gameObject.SetActive(true);

        CompartmentOptions = Vessel.Compartments.Select(compartment => new TMP_Dropdown.OptionData(compartment.name)).ToList();

        foreach (var instruction in vessel.Instructions)
        {
            if (instruction.wayPoint is PortWaypoint)
            {
                GameObject newUI = Instantiate(displayPrefab, content);
                newUI.transform.SetAsLastSibling();
                RouteData data = newUI.GetComponent<RouteData>();

                data.SetDisplay(this);
                data.SetInstruction(instruction);
            }

        }
    }

    public void ExitRoute()
    {
        
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            var t = content.GetChild(i);
            Destroy(t.gameObject);
        }
        content.DetachChildren();

        if (content.TryGetComponent<RefreshablePanel>(out var panel))
            panel.Refresh();

        Vessel = null;

        StatusDisplay.Instance.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }

    public void OnPortPressed(Port port)
    {
        if (Vessel == null) return;

        var instruction = Vessel.CreateInstruction(port.WayPoint);

        GameObject newUI = Instantiate(displayPrefab, content);
        newUI.transform.SetAsLastSibling();
        RouteData data = newUI.GetComponent<RouteData>();

        data.SetDisplay(this);
        data.SetInstruction(instruction);
    }
}
