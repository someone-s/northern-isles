
using System;
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

        if (inputs.Count > 0)
            enabled = false;
        else if (IsReady())
            Produce();
            
        GetState();
    }

    public void SetPort(Port port)
    {
        this.port = port;
    }

    private string cachedState = null;

    public string GetState()
    {
        cachedState = JsonUtility.ToJson(new GeneratorState()
        {
            elapsedS = elapsedS,
            enabled = enabled,
            inputs = inputs.Select(pair => new GeneratorPair() { type = pair.Key, value = pair.Value }).ToList(),
            outputs = outputs.Select(pair => new GeneratorPair() { type = pair.Key, value = pair.Value }).ToList()
        });
        return cachedState;
    }

    public void SetState(string json)
    {
        cachedState = json;
        
        var state = JsonUtility.FromJson<GeneratorState>(cachedState);
        foreach (var pair in state.inputs)
            inputs[pair.type] = pair.value;
        foreach (var pair in state.outputs)
            outputs[pair.type] = pair.value;
        elapsedS = state.elapsedS;
        enabled = state.enabled;
        Debug.Log(elapsedS);
    }

    public void Rollback()
    {
        SetState(cachedState);
    }

    [Serializable]
    private struct GeneratorState
    {
        public float elapsedS;
        public bool enabled;
        public List<GeneratorPair> inputs;
        public List<GeneratorPair> outputs;
    }

    [Serializable]
    private struct GeneratorPair
    {
        public CargoType type;
        public int value;
    }

    private void Start()
    {
        port.Storage.AddUser(this);
    }

    private void OnDestroy()
    {
        port.Storage.RemoveUser(this);
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