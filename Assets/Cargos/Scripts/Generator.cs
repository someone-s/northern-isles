
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;

public class Generator : MonoBehaviour, IPortUser
{
    [SerializeField] private SerializedDictionary<CargoType, int> inputs;
    [SerializeField] private SerializedDictionary<CargoType, int> outputs;

    [SerializeField] private float productionS;
    private float elapsedS;

    [SerializeField] private Port port;

    public UnityEvent<float> OnProgressUpdate;

    private void Awake()
    {
        elapsedS = 0f;
    }

    private void Start()
    {
        port.Storage.AddUser(this);

        if (inputs.Count > 0)
            enabled = false;
        else if (IsReady())
            Produce();
    }

    public bool TryConsume(CargoType cargo)
    {
        if (inputs.TryGetValue(cargo, out int stored) && stored < 1)
        {
            inputs[cargo] = 1;

            if (IsReady())
                Produce();

            return true;
        }
        else
            return false;
    }

    public CargoType? TryTake()
    {
        foreach (var key in new List<CargoType>(outputs.Keys))
        {
            if (outputs[key] > 0)
            {
                outputs[key] = 0;

                if (IsReady())
                    Produce();

                return key;
            }
        }

        return null;
    }

    private bool IsReady()
    {
        foreach (var amount in inputs.Values)
            if (amount < 1)
                return false;
                
        foreach (var amount in outputs.Values)
            if (amount > 0)
                return false;

        return true;
    }

    private void Produce()
    {
        elapsedS = 0f;
        enabled = true;
    }


    private void Update()
    {
        elapsedS += Time.deltaTime;
        if (elapsedS < productionS)
            OnProgressUpdate.Invoke(elapsedS / productionS);
        else
        {
            foreach (var key in new List<CargoType>(inputs.Keys))
            {
                inputs[key] = 0;
            }

            foreach (var key in new List<CargoType>(outputs.Keys))
            {
                if (!port.Storage.TryPut(key))
                    outputs[key] = 1;
            }

            if (inputs.Count > 0)
                enabled = false;
            else if (IsReady())
                Produce();
        }
    }
}