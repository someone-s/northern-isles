public struct Good
{
    public GoodType type;
    public float quantity;

}


public enum GoodType
{
    Passenger = 31,
    Mail = 30,
    Slate = 29,
    Sheep = 28,
    Food = 27
}

public static class GoodTypeExtension
{
    public static int GetLayerMask(this GoodType type) => 1 << (int)type;
}