using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class PortWarehouse : MonoBehaviour
{
    Dictionary<CargoType, PortStorage> storages;

    private void Awake()
    {
        PortStorage[] components = GetComponentsInChildren<PortStorage>();
        storages = new(components.Length);
        foreach (var storage in components)
            storages[storage.GetCargoType()] = storage;
    }

    public void AddCargo(CargoType type, float quantity, out float price)
    {
        storages[type].AddCargo(quantity, out price);
    }

    public void RemoveCargo(CargoType type, float requestQuantity, out float price, out float actualQuantity)
    {
        storages[type].RemoveCargo(requestQuantity, out price, out actualQuantity);
    }
    
}