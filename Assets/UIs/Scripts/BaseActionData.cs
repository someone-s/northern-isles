using UnityEngine;

public abstract class BaseActionData : MonoBehaviour
{
    public RouteData Route { get; private set; }
    public IVesselAction Action { get; private set; }

    public virtual void SetRoute(RouteData route)
    {
        Route = route;
    }

    public virtual void SetAction(IVesselAction action)
    {
        Action = action;
    }

    public virtual void ChangeActionOrder(int index)
    {
        Route.MoveAction(index, Action);
    }
}