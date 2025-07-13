using System;
using System.Collections.Generic;
using UnityEngine;


public class CargoPriceGraph
{
    public static Dictionary<CargoType, CargoPriceGraph> entries = new()
    {
        { CargoType.Wool,       new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Coal,       new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Clothes,    new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
    };

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