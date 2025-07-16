using System.Collections.Generic;
using System.Collections.ObjectModel;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;

public class VesselCompartment : MonoBehaviour
{
    private void Awake()
    {
        RemainingSpace = capacity;
        Cargos = new ReadOnlyDictionary<CargoType, float>(cargos);
    }

    [SerializeField] private float capacity = 5f;
    public float RemainingSpace { get; private set; }
    [SerializeField] private SerializedDictionary<CargoType, float> cargos;
    public ReadOnlyDictionary<CargoType, float> Cargos;

    public UnityEvent<CargoType, float, float> OnLoadCargo;
    public UnityEvent<CargoType, float, float> OnUnloadCargo;


    public void LoadCargo(CargoType type, float availableQuantity, out float actualQuantity)
    {
        actualQuantity = Mathf.Min(availableQuantity, RemainingSpace);

        RemainingSpace -= actualQuantity;
        if (!cargos.TryAdd(type, actualQuantity))
            cargos[type] += actualQuantity;

        OnLoadCargo.Invoke(type, actualQuantity, cargos[type]);
    }

    public void UnloadCargo(CargoType type, float requestQuantity, out float actualQuantity)
    {
        if (cargos.TryGetValue(type, out float storedQuantity))
        {
            actualQuantity = Mathf.Min(requestQuantity, storedQuantity);
            cargos[type] -= actualQuantity;
            RemainingSpace += actualQuantity;

            OnUnloadCargo.Invoke(type, actualQuantity, cargos[type]);
        }
        else
        {
            actualQuantity = 0f;
        }
    }
}