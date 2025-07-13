using System;

[Serializable]
public struct CargoUnit
{
    public CargoType type;
    public float unitPrice;
    public float quantity;
}

[Serializable]
public struct CargoRequirement
{
    public float lowerUnitPrice;
    public float upperUnitPrice;
    public float currentUnitPrice;
    public float quantity;
    public float shortFall;
}


[Serializable]
public enum CargoType
{
    Passenger,
    Cattle,
    Grain,
    Slate,
    Alcohol,
    Sheep,
    Bread,
    Coal,
    Mail,
    Material,
    Meat,
    Wool,
    Clothes,
    Flour
}