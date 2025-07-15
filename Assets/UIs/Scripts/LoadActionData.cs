using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadActionData : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown compartmentDropdown;
    [SerializeField] private TMP_Dropdown cargoDropdown;
    [SerializeField] private TMP_InputField quantityField;

    public RouteData Route { get; private set; }
    public VesselLoadAction Action { get; private set; }

    public void SetRoute(RouteData route)
    {
        Route = route;

        compartmentDropdown.ClearOptions();
        compartmentDropdown.AddOptions(Route.Display.CompartmentOptions);

        cargoDropdown.ClearOptions();
        cargoDropdown.AddOptions(Route.Display.Cargo.Options);
    }

    public void SetData(VesselLoadAction action)
    {
        Action = action;

        compartmentDropdown.value = action.compartmentIndex;

        cargoDropdown.value = Route.Display.Cargo.Indicies[action.cargo];

        quantityField.text = action.amount.ToString();
    }
}