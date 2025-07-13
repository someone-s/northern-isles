
using UnityEngine;

public class VesselUnloadAction : MonoBehaviour, IVesselAction
{    public int compartmentIndex;
    public CargoType cargo;
    public float amount;

    public void PerformAction(Vessel vessel, IWayPoint wayPoint)
    {
        if (wayPoint is not PortWayPoint) return;

        PortWarehouse warehouse = (wayPoint as PortWayPoint).Port.Warehouse;
        VesselCompartment compartment = vessel.Compartments[compartmentIndex];

        compartment.UnLoadCargo(cargo, amount, out float actualQuantity);
        warehouse.AddCargo(cargo, actualQuantity, out float price);
    }
}
