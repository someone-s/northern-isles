using UnityEngine;
using UnityEngine.Events;

public class PortBuilding : MonoBehaviour, IBuilding
{

    private void Awake()
    {
        Collider collider = gameObject.GetComponentInChildren<Collider>();
        collider.includeLayers = GoodType.Slate.GetLayerMask();
    }

    public Vector3 GetLocation() => transform.position;

    public Good Recieve(Good good)
    {
        good.quantity = 0f;
        return good;
    }

    public UnityEvent GetDestroyEvent()
    {
        throw new System.NotImplementedException();
    }

    public void ReturnHome(Courier courier)
    {
        throw new System.NotImplementedException();
    }
}
