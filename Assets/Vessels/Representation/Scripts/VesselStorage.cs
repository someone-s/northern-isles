using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

public class VesselStorage : MonoBehaviour
{
    public Vessel Vessel { get; private set; }

    [field: SerializeField]
    public int Capacity { get; private set; }

    private List<CargoType> storage;
    public IReadOnlyCollection<CargoType> Storage => storage;

    public UnityEvent<IReadOnlyCollection<CargoType>, float> OnStorageChange;
    public UnityEvent OnSatisfiedCargo;
    public UnityEvent OnDissatisfiedCargo;

    private JToken cachedState;

    private void Awake()
    {
        Vessel = GetComponentInParent<Vessel>();

        storage ??= new();
        OnStorageChange ??= new();

        GetState();
    }

    private void Start()
    {
        OnStorageChange.Invoke(storage, Capacity);
    }

    public void Remove(CargoType cargo)
    {
        storage.Remove(cargo);
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

    private void UpdateSatisfaction()
    {
        bool anyConflict()
        {
            foreach (var cargo1 in storage)
                foreach (var cargo2 in storage)
                    if (cargo1.IsConflictingWith(cargo2))
                        return true;

            return false;
        }
        ;
        bool hasConflict = anyConflict();

        if (hasConflict)
            OnDissatisfiedCargo.Invoke();
        else
            OnSatisfiedCargo.Invoke();
    }

    public void Load(Port port)
    {
        IEnumerable<CargoType> acceptTypes = Vessel.Navigation.Ports.Where(p => p != port).SelectMany(p => p.Storage.AcceptList).Distinct();

        port.Storage.Load(cargo => acceptTypes.Contains(cargo), ref storage, Capacity);

        OnStorageChange.Invoke(storage, Capacity);

        UpdateSatisfaction();
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
