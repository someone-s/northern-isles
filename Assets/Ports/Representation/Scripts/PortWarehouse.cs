using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PortWarehouse : MonoBehaviour
{
    Dictionary<CargoType, PortStorage> storages;

    public UnityEvent<CargoType, float> OnCargoPresent;
    public UnityEvent<CargoType, float> OnCargoVaccate;

    private void Awake()
    {
        storages = new();
        foreach (var type in Enum.GetValues(typeof(CargoType)).Cast<CargoType>())
        {
            var storage = gameObject.AddComponent<PortStorage>();
            storage.type = type;
            storages[type] = storage;
        }

        if (OnCargoPresent == null)
            OnCargoPresent = new();
        if (OnCargoVaccate == null)
            OnCargoVaccate = new();
    }

    public void AddCargo(CargoType type, float quantity, out float price)
    {
        storages[type].AddCargo(quantity, out float total, out price);
        OnCargoPresent.Invoke(type, total);
    }

    public void RemoveCargo(CargoType type, float requestQuantity, out float price, out float actualQuantity)
    {
        storages[type].RemoveCargo(requestQuantity, out float total, out price, out actualQuantity);
        OnCargoVaccate.Invoke(type, total);
    }

}