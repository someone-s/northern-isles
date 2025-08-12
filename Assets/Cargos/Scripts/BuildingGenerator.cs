
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class BuildingGenerator : SerializedMonoBehaviour, IPortUser
{
    [SerializeField] private Dictionary<CargoType, int> inputs;
    [SerializeField] private Dictionary<CargoType, int> outputs;

    [SerializeField] private float productionS;
    private float elapsedS;

    private Port port;

    public UnityEvent<float> OnProgressUpdate;
    public UnityEvent OnProductionBegin;

    private JToken cachedState = null;

    public void Setup(Port port)
    {
        this.port = port;

        port.Storage.AddUser(this);

        if (IsReady())
            Produce();
    }

    public JToken GetState()
    {
        cachedState = JToken.FromObject(new GeneratorState()
        {
            elapsedS = elapsedS,
            enabled = enabled,
            inputs = inputs.Select(pair => new GeneratorPair() { type = pair.Key, value = pair.Value }).ToList(),
            outputs = outputs.Select(pair => new GeneratorPair() { type = pair.Key, value = pair.Value }).ToList()
        });
        return cachedState;
    }

    public void SetState(JToken json)
    {
        cachedState = json;

        var state = cachedState.ToObject<GeneratorState>();
        foreach (var pair in state.inputs)
            inputs[pair.type] = pair.value;
        foreach (var pair in state.outputs)
            outputs[pair.type] = pair.value;
        elapsedS = state.elapsedS;
        enabled = state.enabled;
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

    private void OnDestroy()
    {
        port.Storage.RemoveUser(this);
    }

    public IReadOnlyCollection<CargoType> GetAcceptList() => inputs.Keys;

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

        OnProductionBegin.Invoke();
    }


    private void Update()
    {
        elapsedS += Time.deltaTime * SpeedControl.Instance.TimeScale;
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

            if (IsReady())
                Produce();
            else
            {
                elapsedS = 0f;
                OnProgressUpdate.Invoke(0f);
                enabled = false;
            }
        }
    }
}