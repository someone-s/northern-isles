using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnloadActionData : BaseActionData
{
    [SerializeField] private TMP_Dropdown compartmentDropdown;
    [SerializeField] private TMP_Dropdown cargoDropdown;
    [SerializeField] private TMP_InputField quantityField;

    public UnityEvent<int> OnCompartmentIndexChanged;
    public UnityEvent<CargoType> OnCargoChanged;
    public UnityEvent<float> OnQuantityChanged;
    protected override void Awake()
    {
        base.Awake();

        compartmentDropdown.onValueChanged.AddListener(CompartmentChange);
        cargoDropdown.onValueChanged.AddListener(CargoChange);
        quantityField.onEndEdit.AddListener(QuantityChange);

        if (OnCompartmentIndexChanged == null)
            OnCompartmentIndexChanged = new();
        if (OnCargoChanged == null)
            OnCargoChanged = new();
        if (OnQuantityChanged == null)
            OnQuantityChanged = new();
    }

    private void OnDestroy()
    {
        compartmentDropdown.onValueChanged.RemoveListener(CompartmentChange);
        cargoDropdown.onValueChanged.RemoveListener(CargoChange);
        quantityField.onEndEdit.RemoveListener(QuantityChange);
    }

    private void CompartmentChange(int index)
    {
        var unloadAction = Action as VesselUnloadAction;
        unloadAction.CompartmentIndex = index;

        OnCompartmentIndexChanged.Invoke(unloadAction.CompartmentIndex);
    }
    private void CargoChange(int index)
    {
        var unloadAction = Action as VesselUnloadAction;
        unloadAction.Cargo = Route.Display.Cargo.Cargos[index];

        OnCargoChanged.Invoke(unloadAction.Cargo);
    }
    private void QuantityChange(string input)
    {
        if (float.TryParse(input, out float quantity))
        {
            var unloadAction = Action as VesselUnloadAction;
            unloadAction.Amount = quantity;

            OnQuantityChanged.Invoke(unloadAction.Amount);
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
        var unloadAction = action as VesselUnloadAction;

        compartmentDropdown.value = unloadAction.CompartmentIndex;

        cargoDropdown.value = Route.Display.Cargo.Indicies[unloadAction.Cargo];

        quantityField.text = unloadAction.Amount.ToString();
    }
}