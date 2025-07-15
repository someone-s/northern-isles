using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnloadActionData : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown compartmentDropdown;
    [SerializeField] private TMP_Dropdown cargoDropdown;
    [SerializeField] private TMP_InputField quantityField;

    public RouteData Route { get; private set; }
    public VesselUnloadAction Action { get; private set; }

    public void SetRoute(RouteData route)
    {
        Route = route;
    }

    public void SetData(VesselUnloadAction action)
    {
        Action = action;

        compartmentDropdown.ClearOptions();
        compartmentDropdown.AddOptions(Route.Display.CompartmentOptions);
        compartmentDropdown.value = action.compartmentIndex;

        cargoDropdown.ClearOptions();
        cargoDropdown.AddOptions(Route.Display.Cargo.Options);
        cargoDropdown.value = Route.Display.Cargo.Indicies[action.cargo];

        quantityField.text = action.amount.ToString();
    }
}