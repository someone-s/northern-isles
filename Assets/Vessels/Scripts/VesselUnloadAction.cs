
using UnityEngine;

public class VesselUnloadAction : MonoBehaviour, IVesselAction
{    public int compartmentIndex;
    public CargoType cargo;
    public float amount;

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
