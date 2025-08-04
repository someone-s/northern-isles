using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Market : MonoBehaviour
{
    public static Market Instance { get; private set; }

    private Market()
    {
        Instance = this;
    }

    [SerializeField]
    private Dictionary<CargoType, CargoPriceGraph> entries = new()
    {
        { CargoType.Passenger,  new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Cattle,     new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Grain,      new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Slate,      new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Alcohol,    new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Sheep,      new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Bread,      new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Coal,       new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Mail,       new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Material,   new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Meat,       new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Wool,       new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Clothes,    new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
        { CargoType.Flour,      new() { supplySlope = 0.5f, supplyOffset = 0f, demandSlope = -0.5f, demandOffset = 1f, minPrice = 0.1f, maxPrice = 2f } },
    };

    public ReadOnlyDictionary<CargoType, CargoPriceGraph> Entries { get; private set; }

    private void Awake()
    {
        Entries = new(entries);
    }
}