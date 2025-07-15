using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortWarehouse : MonoBehaviour
{
    Dictionary<CargoType, PortStorage> storages;

    private void Awake()
    {
        storages = new();
        foreach (var type in Enum.GetValues(typeof(CargoType)).Cast<CargoType>())
        {
            var storage = gameObject.AddComponent<PortStorage>();
            storage.type = type;
            storages[type] = storage;
        }
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