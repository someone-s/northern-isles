using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PortStorage : MonoBehaviour
{
    [field: SerializeField]
    public int Capacity { get; private set; }
    private Queue<CargoType> outbounds;

    private List<IPortUser> portUsers;

    private string cachedState = null;

    public UnityEvent<IReadOnlyCollection<CargoType>> OnOutboundChange;

    private void Awake()
    {
        outbounds ??= new();
        portUsers ??= new();
        GetState();
    }

    private void Start()
    {
        OnOutboundChange.Invoke(outbounds);
    }

    public void AddUser(IPortUser user)
    {
        portUsers.Add(user);
    }
    public void RemoveUser(IPortUser user)
    {
        portUsers.Remove(user);
    }


    public bool Unload(CargoType cargo)
    {
        foreach (var user in portUsers)
            if (user.TryConsume(cargo))
                return true;

        return false;
    }

    public bool TryPut(CargoType cargo)
    {
        if (outbounds.Count < Capacity)
        {
            outbounds.Enqueue(cargo);
            OnOutboundChange.Invoke(outbounds);

            return true;
        }
        return false;
    }

    private void Replenish()
    {
        while (true)
        {
            bool allEmpty = true;
            foreach (var user in portUsers)
            {
                if (outbounds.Count >= Capacity)
                    return;

                var result = user.TryTake();
                if (result != null)
                {
                    outbounds.Enqueue(result.Value);
                    OnOutboundChange.Invoke(outbounds);

                    allEmpty = false;
                }

            }
            if (allEmpty)
                return;
        }
    }

    public CargoType? Load()
    {
        if (outbounds.TryDequeue(out CargoType result))
        {
            OnOutboundChange.Invoke(outbounds);

            Replenish();

            return result;
        }
        else
            return null;

    }

    public string GetState()
    {
        cachedState = JsonUtility.ToJson(new StorageState()
        {
            queue = outbounds.ToList()
        });
        return cachedState;
    }

    public void SetState(string json)
    {
        cachedState = json;

        var state = JsonUtility.FromJson<StorageState>(cachedState);
        outbounds.Clear();
        foreach (var outbound in state.queue)
            outbounds.Enqueue(outbound);

        OnOutboundChange.Invoke(outbounds);
    }

    public void Rollback()
    {
        SetState(cachedState);
    }

    private struct StorageState
    {
        public List<CargoType> queue;
    }
}

public interface IPortUser
{
    public bool TryConsume(CargoType cargo);
    public CargoType? TryTake();
}