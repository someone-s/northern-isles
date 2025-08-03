using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortStorage : MonoBehaviour
{
    [field:SerializeField]
    public int Capacity { get; private set; }
    private Queue<CargoType> outbounds;

    private List<IPortUser> portUsers;

    public UnityEvent<IReadOnlyCollection<CargoType>> OnOutboundChange;

    private void Awake()
    {
        outbounds ??= new();
        portUsers ??= new();
    }

    private void Start()
    {
        OnOutboundChange.Invoke(outbounds);
    }

    public void AddUser(IPortUser user)
    {
        portUsers.Add(user);
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
}

public interface IPortUser
{
    public bool TryConsume(CargoType cargo);
    public CargoType? TryTake();
}