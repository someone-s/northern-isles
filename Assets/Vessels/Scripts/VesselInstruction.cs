using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;
using UnityEngine.Events;

public class VesselInstruction : MonoBehaviour
{
    public IWaypoint wayPoint;

    public List<IVesselAction> actions;

    public UnityEvent<IVesselAction> OnAddAction;
    public UnityEvent<VesselInstruction> OnOrderAction;

    private void Awake()
    {
        if (actions == null)
            actions = new();
        if (OnAddAction == null)
            OnAddAction = new();
        if (OnOrderAction == null)
            OnOrderAction = new();
    }

    public IVesselAction AddAction(System.Type type)
    {
        if (typeof(IVesselAction).IsAssignableFrom(type))
        {
            var action = gameObject.AddComponent(type) as IVesselAction;
            actions.Add(action);

            OnAddAction.Invoke(action as IVesselAction);
            OnOrderAction.Invoke(this);
            
            return action;
        }
        else
        {
            return null;
        }
    }

    public void MoveAction(int index, IVesselAction action)
    {
        actions.Remove(action);
        actions.Insert(index, action);

        OnOrderAction.Invoke(this);
    }

    public void DeleteAction(IVesselAction action)
    {
        actions.Remove(action);

        OnOrderAction.Invoke(this);
    }
}

public interface IVesselAction
{
    public void PerformAction(Vessel vessel, IWaypoint wayPoint);
}

