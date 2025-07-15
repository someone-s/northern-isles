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
}

public interface IVesselAction
{
    public void PerformAction(Vessel vessel, IWaypoint wayPoint);
}

