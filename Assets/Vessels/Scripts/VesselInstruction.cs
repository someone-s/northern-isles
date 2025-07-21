using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;
using UnityEngine.Events;

public class VesselInstruction : MonoBehaviour
{
    [RequireInterface(typeof(IWaypoint))]
    public MonoBehaviour wayPoint;

    [RequireInterface(typeof(IVesselAction))]
    public List<MonoBehaviour> actions;

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

    public MonoBehaviour AddAction(System.Type type)
    {
        if (typeof(IVesselAction).IsAssignableFrom(type) && type.IsSubclassOf(typeof(MonoBehaviour)))
        {
            var action = gameObject.AddComponent(type) as MonoBehaviour;
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
        var actionObject = action as MonoBehaviour;
        actions.Remove(actionObject);
        actions.Insert(index, actionObject);

        OnOrderAction.Invoke(this);
    }

    public void DeleteAction(IVesselAction action)
    {
        var actionObject = action as MonoBehaviour;
        actions.Remove(actionObject);

        OnOrderAction.Invoke(this);
    }
}

public interface IVesselAction
{
    public void PerformAction(Vessel vessel, IWaypoint wayPoint);
}

