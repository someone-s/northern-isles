using UnityEngine;

public class VesselLoadAction : MonoBehaviour, IVesselAction
{
    public int compartmentIndex;
    public CargoType cargo;
    public float amount;

    public void PerformAction(Vessel vessel, IWayPoint wayPoint)
    {
        if (wayPoint is not PortWayPoint) return;

        PortWarehouse warehouse = (wayPoint as PortWayPoint).Port.Warehouse;
        VesselCompartment compartment = vessel.Compartments[compartmentIndex];

        float requestQuantity = Mathf.Min(amount, compartment.RemainingSpace);

        warehouse.RemoveCargo(cargo, requestQuantity, out float price, out float actualQuantity);
        compartment.LoadCargo(cargo, actualQuantity, out float loadedQuantity);
    }
}