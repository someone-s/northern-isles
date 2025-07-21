using TMPro;
using UnityEngine;

public class LoadActionData : BaseActionData
{
    [SerializeField] private TMP_Dropdown compartmentDropdown;
    [SerializeField] private TMP_Dropdown cargoDropdown;
    [SerializeField] private TMP_InputField quantityField;

    protected override void Awake()
    {
        base.Awake();

        compartmentDropdown.onValueChanged.AddListener(CompartmentChange);
        cargoDropdown.onValueChanged.AddListener(CargoChange);
        quantityField.onEndEdit.AddListener(QuantityChange);
    }

    private void OnDestroy()
    {
        compartmentDropdown.onValueChanged.RemoveListener(CompartmentChange);
        cargoDropdown.onValueChanged.RemoveListener(CargoChange);
        quantityField.onEndEdit.RemoveListener(QuantityChange);
    }

    private void CompartmentChange(int index)
    {
        var loadAction = Action as VesselLoadAction;
        loadAction.CompartmentIndex = index;
    }
    private void CargoChange(int index)
    {
        var loadAction = Action as VesselLoadAction;
        loadAction.Cargo = Route.Display.Cargo.Cargos[index];
    }
    private void QuantityChange(string input)
    {
        if (float.TryParse(input, out float quantity))
        {
            var loadAction = Action as VesselLoadAction;
            loadAction.Amount = quantity;
        }
    }

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

        compartmentDropdown.value = loadAction.CompartmentIndex;

        cargoDropdown.value = Route.Display.Cargo.Indicies[loadAction.Cargo];

        quantityField.text = loadAction.Amount.ToString();
    }
}