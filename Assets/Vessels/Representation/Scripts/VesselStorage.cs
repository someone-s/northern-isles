using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

public class VesselStorage : MonoBehaviour
{
    [field: SerializeField]
    public int Capacity { get; private set; }

    private List<CargoType> storage;

    public UnityEvent<IReadOnlyCollection<CargoType>, float> OnStorageChange;

    private JToken cachedState;

    private void Awake()
    {
        storage ??= new();
        OnStorageChange ??= new();

        GetState();
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

    public JToken GetState()
    {
        cachedState = JToken.FromObject(new StorageState()
        {
            storage = storage
        });
        return cachedState;
    }

    public void SetState(JToken json)
    {
        cachedState = json;

        var state = cachedState.ToObject<StorageState>();
        storage.Clear();
        foreach (var stored in state.storage)
            storage.Add(stored);

        OnStorageChange.Invoke(storage, Capacity);
    }

    public void Rollback()
    {
        SetState(cachedState);
    }

    private struct StorageState
    {
        public List<CargoType> storage;
    }
}
