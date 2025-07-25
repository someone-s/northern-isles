using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LoadActionData : BaseActionData
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
        var loadAction = Action as VesselLoadAction;
        loadAction.CompartmentIndex = index;

        OnCompartmentIndexChanged.Invoke(loadAction.CompartmentIndex);
    }
    private void CargoChange(int index)
    {
        var loadAction = Action as VesselLoadAction;
        loadAction.Cargo = SpriteCargo.Instance.Cargos[index];

        OnCargoChanged.Invoke(loadAction.Cargo);
    }
    private void QuantityChange(string input)
    {
        if (float.TryParse(input, out float quantity))
        {
            var loadAction = Action as VesselLoadAction;
            loadAction.Amount = quantity;
            
            OnQuantityChanged.Invoke(loadAction.Amount);
        }
    }

    public override void SetRoute(RouteData route)
    {
        base.SetRoute(route);

        compartmentDropdown.ClearOptions();
        compartmentDropdown.AddOptions(Route.Display.CompartmentOptions);

        cargoDropdown.ClearOptions();
        cargoDropdown.AddOptions(SpriteCargo.Instance.Options);
    }

    public override void SetAction(IVesselAction action)
    {
        base.SetAction(action);

        var loadAction = Action as VesselLoadAction;

        compartmentDropdown.value = loadAction.CompartmentIndex;

        cargoDropdown.value = SpriteCargo.Instance.Indicies[loadAction.Cargo];

        quantityField.text = loadAction.Amount.ToString();
    }
}