using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class VesselCompartment : MonoBehaviour
{
    public Vessel Vessel { get; private set; }
    private void Awake()
    {
        Vessel = GetComponentInParent<Vessel>();
        RemainingSpace = capacity;
    }

    [SerializeField] private float capacity = 5f;
    public float RemainingSpace { get; private set; }
    [SerializeField] private SerializedDictionary<CargoType, float> cargos;


    public void LoadCargo(CargoType type, float availableQuantity, out float actualQuantity)
    {
        actualQuantity = Mathf.Min(availableQuantity, RemainingSpace);

        RemainingSpace -= actualQuantity;
        if (!cargos.TryAdd(type, actualQuantity))
            cargos[type] += actualQuantity;
    }

    public void UnLoadCargo(CargoType type, float requestQuantity, out float actualQuantity)
    {
        if (cargos.TryGetValue(type, out float storedQuantity))
        {
            actualQuantity = Mathf.Min(requestQuantity, storedQuantity);
            cargos[type] -= actualQuantity;
            RemainingSpace += actualQuantity;
        }
        else
        {
            actualQuantity = 0f;
        }
    }
}