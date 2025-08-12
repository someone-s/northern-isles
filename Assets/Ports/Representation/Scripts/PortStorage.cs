using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class PortStorage : MonoBehaviour
{
    [field: SerializeField]
    public int Capacity { get; private set; }
    private List<CargoType> outbounds;

    private HashSet<IPortUser> portUsers;

    private JToken cachedState = null;

    private HashSet<CargoType> acceptList;
    public IReadOnlyCollection<CargoType> AcceptList => acceptList;

    public UnityEvent<IReadOnlyCollection<CargoType>> OnOutboundReset;
    public UnityEvent<IReadOnlyCollection<CargoType>> OnOutboundChange;

    private void Awake()
    {
        outbounds ??= new(Capacity);
        portUsers ??= new();
        acceptList ??= new();
    }

    private void Start()
    {
        OnOutboundReset.Invoke(outbounds);
    }

    public void AddUser(IPortUser user)
    {
        portUsers.Add(user);
        acceptList.UnionWith(user.GetAcceptList());
        
    }
    public void RemoveUser(IPortUser user)
    {
        portUsers.Remove(user);

        acceptList.Clear();
        foreach (var portUser in portUsers)
            acceptList.UnionWith(portUser.GetAcceptList());
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
            outbounds.Add(cargo);
            OnOutboundChange.Invoke(outbounds);

            return true;
        }
        return false;
    }

    private bool Replenish()
    {
        bool modified = false;

        while (true)
        {
            bool allEmpty = true;
            foreach (var user in portUsers)
            {
                if (outbounds.Count >= Capacity)
                    return modified;

                var result = user.TryTake();
                if (result != null)
                {
                    outbounds.Add(result.Value);
                    OnOutboundChange.Invoke(outbounds);
                    modified = true;

                    allEmpty = false;
                }

            }
            if (allEmpty)
                return modified;
        }
    }

    public void Load(Func<CargoType, bool> filter, ref List<CargoType> output, int outputCapacity)
    {
        while (output.Count < outputCapacity)
        {
            for (int i = 0; i < outbounds.Count && output.Count < outputCapacity; i++)
            {
                if (filter(outbounds[i]))
                {
                    output.Add(outbounds[i]);
                    outbounds.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            if (!Replenish())
                break;
        }

        OnOutboundChange.Invoke(outbounds);
    }

    public JToken GetState()
    {
        cachedState = JToken.FromObject(new StorageState()
        {
            queue = outbounds.ToList()
        });
        return cachedState;
    }

    public void SetState(JToken json)
    {
        cachedState = json;

        var state = cachedState.ToObject<StorageState>();
        outbounds.Clear();
        foreach (var outbound in state.queue)
            outbounds.Add(outbound);

        OnOutboundReset.Invoke(outbounds);
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
    public IReadOnlyCollection<CargoType> GetAcceptList();
}