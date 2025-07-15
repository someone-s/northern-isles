using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;

public class VesselInstruction : MonoBehaviour
{
    [RequireInterface(typeof(IWaypoint))]
    public Object wayPoint;

    [RequireInterface(typeof(IVesselAction))]
    public List<Object> actions;

    public Object AddAction(System.Type type)
    {
        if (typeof(IVesselAction).IsAssignableFrom(type))
        {
            Component action = gameObject.AddComponent(type);
            actions.Add(action);
            return action;
        }
        else
        {
            return null;
        }
    }

    public void MoveAction(int index, IVesselAction action)
    {
        var actionObject = action as Object;
        actions.Remove(actionObject);
        actions.Insert(index, actionObject);
    }

    public void DeleteAction(IVesselAction action)
    {
        var actionObject = action as Object;
        actions.Remove(actionObject);
    }
}

public interface IVesselAction
{
    public void PerformAction(Vessel vessel, IWaypoint wayPoint);
}

