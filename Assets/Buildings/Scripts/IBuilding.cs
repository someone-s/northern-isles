using UnityEngine;
using UnityEngine.Events;

public interface IBuilding
{
    public Vector3 GetLocation();

    public Good Recieve(Good good);

    public void ReturnHome(Courier courier);

    public UnityEvent GetDestroyEvent();
}
