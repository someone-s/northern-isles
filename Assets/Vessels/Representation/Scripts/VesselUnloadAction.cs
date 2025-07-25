
using UnityEngine;
using UnityEngine.Events;

public class VesselUnloadAction : MonoBehaviour, IVesselAction
{
    private int compartmentIndex;
    public int CompartmentIndex
    {
        get => compartmentIndex;
        set
        {
            compartmentIndex = value;
            OnModifiedData.Invoke(this);
        }
    }
    private CargoType cargo;
    public CargoType Cargo
    {
        get => cargo;
        set
        {
            cargo = value;
            OnModifiedData.Invoke(this);
        }
    }
    private float amount;
    public float Amount
    {
        get => amount;
        set
        {
            amount = value;
            OnModifiedData.Invoke(this);
        }
    }

    public UnityEvent<VesselUnloadAction> OnModifiedData;

    private void Awake()
    {
        if (OnModifiedData == null)
            OnModifiedData = new();
    }

    public void PerformAction(Vessel vessel, IWaypoint wayPoint)
    {
        if (wayPoint is not PortWaypoint) return;

        PortWarehouse warehouse = (wayPoint as PortWaypoint).Port.Warehouse;
        VesselCompartment compartment = vessel.Compartments[compartmentIndex];

        compartment.UnloadCargo(cargo, amount, out float actualQuantity);
        warehouse.AddCargo(cargo, actualQuantity, out float price);

        vessel.Account.AddTransaction("Income", new VesselCargoTransaction(cargo, actualQuantity), price);
    }
}
