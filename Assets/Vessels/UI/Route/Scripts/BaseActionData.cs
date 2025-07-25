using UnityEngine;

public abstract class BaseActionData : MonoBehaviour
{
    public RouteData Route { get; private set; }
    public IVesselAction Action { get; private set; }

    protected virtual void Awake()
    {
        var reordable = GetComponent<ReorderablePanel>();
        if (reordable != null)
            reordable.OnMove.AddListener(ChangeActionOrder);
    }

    public virtual void SetRoute(RouteData route)
    {
        Route = route;
    }

    public virtual void SetAction(IVesselAction action)
    {
        Action = action;
    }

    public void ChangeActionOrder(int index)
    {
        Route.MoveAction(index, Action);
    } 
    public void ChangeActionOrder(int _, int index)
    {
        Route.MoveAction(index, Action);
    }

    public void DeleteAction()
    {
        Route.DeleteAction(Action);


        var deletablePanel = GetComponent<DeletablePanel>();
        if (deletablePanel != null)
            deletablePanel.Delete();
    }
}