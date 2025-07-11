using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class House : MonoBehaviour, IBuilding
{
    private void Awake()
    {
        Collider collider = gameObject.GetComponentInChildren<Collider>();
        collider.includeLayers = GoodType.Food.GetLayerMask();
    }

    private Stack<Courier> availableCouriers = new();
    [SerializeField] private GameObject courierPrefab;
    [SerializeField] private float waitTimeS = 10f;

    private float elapsedS = 0f;
    private void Update()
    {
        elapsedS += Time.deltaTime;

        if (elapsedS > waitTimeS)
        {
            elapsedS = 0f;

            Courier courier;
            if (availableCouriers.Count < 1)
            {
                GameObject courierObject = Instantiate(courierPrefab);
                courier = courierObject.GetComponent<Courier>();
                courier.SetHome(this);
            }
            else
            {
                courier = availableCouriers.Pop();
            }

            courier.FindDestination(new Good()
            {
                type = GoodType.Slate,
                quantity = 100f
            });
            
        }
    }

    public Vector3 GetLocation() => transform.position;

    public Good Recieve(Good good)
    {
        good.quantity = 0f;
        return good;
    }

    public void ReturnHome(Courier courier)
    {
        availableCouriers.Push(courier);
    }

    public UnityEvent GetDestroyEvent()
    {
        throw new System.NotImplementedException();
    }
}
