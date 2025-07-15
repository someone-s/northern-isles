using TMPro;
using UnityEngine;

public class LoadActionData : BaseActionData
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

        var loadAction = Action as VesselLoadAction;

        compartmentDropdown.value = loadAction.compartmentIndex;

        cargoDropdown.value = Route.Display.Cargo.Indicies[loadAction.cargo];

        quantityField.text = loadAction.amount.ToString();
    }
}