using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;

[System.Serializable]
public class VesselInstruction
{
    [RequireInterface(typeof(IWayPoint))]
    public Object wayPoint;

    [RequireInterface(typeof(IVesselAction))]
    public List<Object> actions;
}

public interface IVesselAction
{
    public void PerformAction(Vessel vessel, IWayPoint wayPoint);
}

