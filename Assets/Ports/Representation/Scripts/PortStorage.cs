using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PortStorage : MonoBehaviour
{
    public CargoType type;

    [SerializeField] private float logPeriod = 20f;

    [Header("Debug")]
    [SerializeField] private float supplyPerS = 0f;
    [SerializeField] private float demandPerS = 0f;
    [SerializeField] private float unitPrice = 0f;
    [SerializeField] private float storedQuantity = 0f;
    [SerializeField] private float lastUpdateS = 0f;
    [SerializeField] private LinkedList<Log> supplyLogs = new();
    [SerializeField] private LinkedList<Log> demandLogs = new();


    
    public class Log
    {
        public double timestamp;
        public float quantity;
    }

    public void AddCargo(float quantity, out float total, out float price)
    {
        supplyLogs.AddLast(new Log { timestamp = 0f, quantity = quantity });
        storedQuantity += quantity;

        UpdatePrice();
        price = unitPrice * quantity;

        total = storedQuantity;
    }
    public void UpdatePrice()
    {
        float elapsedS = Time.timeSinceLevelLoad - lastUpdateS;
        lastUpdateS = Time.timeSinceLevelLoad;

        while (supplyLogs.Count > 0 && supplyLogs.First().timestamp > logPeriod - elapsedS)
            supplyLogs.RemoveFirst();
        float supplyInPeriod = 0f;
        foreach (var log in supplyLogs)
        {
            supplyInPeriod += log.quantity;
            log.timestamp += elapsedS;
        }
        supplyPerS = supplyInPeriod / logPeriod;

        while (demandLogs.Count > 0 && demandLogs.First().timestamp > logPeriod - elapsedS)
            demandLogs.RemoveFirst();
        float demandInPeriod = 0f;
        foreach (var log in demandLogs)
        {
            demandInPeriod += log.quantity;
            log.timestamp += elapsedS;
        }
        demandPerS = demandInPeriod / logPeriod;

        unitPrice = Market.Instance.Entries[type].Sample(supplyPerS, demandPerS);
    }

    public void RemoveCargo(float requestQuantity, out float total, out float price, out float actualQuantity)
    {
        demandLogs.AddLast(new Log { timestamp = 0f, quantity = requestQuantity });
        actualQuantity = Mathf.Min(requestQuantity, storedQuantity);
        storedQuantity -= actualQuantity;

        UpdatePrice();
        price = unitPrice * actualQuantity;

        total = storedQuantity;
    }
}