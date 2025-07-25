public struct VesselCargoTransaction
{
    public readonly CargoType type;
    public readonly float amount;

    public VesselCargoTransaction(CargoType type, float amount)
    {
        this.type = type;
        this.amount = amount;
    }
}