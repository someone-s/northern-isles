using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnloadActionData : BaseActionData
{
    [SerializeField] private TMP_Dropdown compartmentDropdown;
    [SerializeField] private TMP_Dropdown cargoDropdown;
    [SerializeField] private TMP_InputField quantityField;

    public override void SetRoute(RouteData route)
    {
        base.SetRoute(route);

        compartmentDropdown.ClearOptions();
        compartmentDropdown.AddOptions(Route.Display.CompartmentOptions);

        cargoDropdown.ClearOptions();
        cargoDropdown.AddOptions(Route.Display.Cargo.Options);
    }

    public override void SetAction(IVesselAction action)
    {
        base.SetAction(action);
        var unloadAction = action as VesselUnloadAction;

        compartmentDropdown.value = unloadAction.compartmentIndex;

        cargoDropdown.value = Route.Display.Cargo.Indicies[unloadAction.cargo];

        quantityField.text = unloadAction.amount.ToString();
    }
}