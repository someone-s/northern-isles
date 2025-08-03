using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VesselStorage : MonoBehaviour
{
    [field: SerializeField]
    public int Capacity { get; private set; }

    private List<CargoType> storage;

    public UnityEvent<IReadOnlyCollection<CargoType>, float> OnStorageChange;

    private void Awake()
    {
        storage ??= new();
        OnStorageChange ??= new();
    }

    private void Start()
    {
        OnStorageChange.Invoke(storage, Capacity);
    }

    public void Unload(Port port)
    {
        for (int i = 0; i < storage.Count;)
        {
            if (port.Storage.Unload(storage[i]))
                storage.RemoveAt(i);
            else
                i++;
        }
        OnStorageChange.Invoke(storage, Capacity);
    }

    public void Load(Port port)
    {
        while (storage.Count < Capacity)
        {
            var result = port.Storage.Load();
            if (result == null)
                break;
            else
                storage.Add(result.Value);
        }

        OnStorageChange.Invoke(storage, Capacity);
    }
}
