using System;
using UnityEngine;


[Serializable]
public class CargoPriceGraph
{
    public float supplySlope;
    public float supplyOffset;

    public float demandSlope;
    public float demandOffset;

    public float maxPrice;
    public float minPrice;

    public float Sample(float supplyShift, float demandShift)
    {
        float supplyLocation = supplyOffset - supplyShift * supplySlope;
        float demandLocation = demandOffset - demandShift * demandSlope;

        float lhs = supplySlope - demandSlope;
        float rhs = demandLocation - supplyLocation;

        float x = rhs / lhs;
        float y = x * supplySlope + supplyLocation;

        return Mathf.Clamp(y, minPrice, maxPrice);
    }
}

[Serializable]
public class CargoRequirement
{
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