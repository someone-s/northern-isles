
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<CargoType, CargoRequirement> inputs;
    [SerializeField] private SerializedDictionary<CargoType, float> outputs;
    [SerializeField] private float rateS;
    [SerializeField] private float baseCost;
    [SerializeField] private float aggregateCost;
    private float elapsedS;

    private Port port;
    private void Awake()
    {
        port = GetComponentInParent<Port>();
        aggregateCost = 0;
        elapsedS = Random.Range(0f, rateS);
    }


    private void Update()
    {
        elapsedS += Time.deltaTime;
        if (elapsedS < rateS) return;
        elapsedS = 0f;

        List<CargoType> inputKeys = inputs.Keys.ToList();

        foreach (var type in inputKeys)
        {
            port.Warehouse.RemoveCargo(type, inputs[type].shortFall, out float actualCost, out float actualQuantity);

            aggregateCost += actualCost;
            inputs[type].shortFall -= actualQuantity;

        }

        bool metAllRequirements = true;
        foreach (var type in inputKeys)
        {
            var requirement = inputs[type];
            if (requirement.shortFall > 0f)
                metAllRequirements = false;
        }

        if (metAllRequirements)
        {
            float aggregateRevenue = 0f;
            foreach (var output in outputs)
            {
                port.Warehouse.AddCargo(output.Key, output.Value, out float revenue);
                aggregateRevenue += revenue;
            }

            foreach (var type in inputKeys)
            {
                var requirement = inputs[type];
                requirement.shortFall = requirement.quantity;
                inputs[type] = requirement;
            }

            aggregateCost = 0f;
        }
    }
}