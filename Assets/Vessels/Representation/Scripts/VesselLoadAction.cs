using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class VesselLoadAction : MonoBehaviour, IVesselAction
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

    public UnityEvent<VesselLoadAction> OnModifiedData;

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

        float requestQuantity = Mathf.Min(amount, compartment.RemainingSpace);

        warehouse.RemoveCargo(cargo, requestQuantity, out float price, out float actualQuantity);
        compartment.LoadCargo(cargo, actualQuantity, out float loadedQuantity);
        Assert.AreEqual(actualQuantity, loadedQuantity);

        vessel.Account.AddTransaction("Expense", new VesselCargoTransaction(cargo, actualQuantity), -price);
    }
}