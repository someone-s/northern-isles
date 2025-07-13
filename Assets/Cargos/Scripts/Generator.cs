using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Unity.Collections;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<CargoType, CargoRequirement> inputs;
    private CargoType[] inputKeys;
    [SerializeField] private CargoUnit[] outputs;
    [SerializeField] private float rateS;
    private float elapsedS;

    private Port port;
    private void Awake()
    {
        port = GetComponentInParent<Port>();
        inputKeys = new CargoType[inputs.Count];
        inputs.Keys.CopyTo(inputKeys, 0);
        elapsedS = UnityEngine.Random.Range(0f, rateS);
    }


    private void Update()
    {
        elapsedS += Time.deltaTime;
        if (elapsedS < rateS) return;
        elapsedS = 0f;

        foreach (var type in inputKeys)
        {
            for (int i = 0; i < port.Warehouse.storedCargos.Count; i++)
            {
                var unit = port.Warehouse.storedCargos[i];
                if (unit.type != type) continue;

                var requirement = inputs[type];
                if (unit.quantity > requirement.shortFall)
                {
                    unit.quantity -= requirement.shortFall;
                    port.Warehouse.storedCargos[i] = unit;

                    requirement.shortFall = 0f;
                    inputs[type] = requirement;

                    break;
                }
                else
                {
                    port.Warehouse.storedCargos.RemoveAt(i);
                    i--;

                    requirement.shortFall -= unit.quantity;
                    inputs[type] = requirement;
                }
            }
        }

        bool metAllRequirements = true;
        float totalPrice = 0f;
        foreach (var type in inputKeys)
        {
            var requirement = inputs[type];
            if (requirement.shortFall > 0f)
            {
                requirement.currentUnitPrice += 0.1f;
                if (requirement.currentUnitPrice > requirement.upperUnitPrice) requirement.currentUnitPrice = requirement.upperUnitPrice;
                inputs[type] = requirement;

                metAllRequirements = false;
            }
            else
            {
                requirement.currentUnitPrice -= 0.1f;
                if (requirement.currentUnitPrice < requirement.lowerUnitPrice) requirement.currentUnitPrice = requirement.lowerUnitPrice;
                inputs[type] = requirement;
            }

            totalPrice += requirement.currentUnitPrice * requirement.quantity;
        }
        
        float totalUnit = 0f;
        for (int i = 0; i < outputs.Length; i++)
            totalUnit += outputs[i].quantity;

        float unitPrice = totalPrice / totalUnit;
        for (int i = 0; i < outputs.Length; i++)
            outputs[i].unitPrice = unitPrice;

        if (metAllRequirements)
        {

            port.Warehouse.storedCargos.AddRange(outputs);

            foreach (var type in inputKeys)
            {
                var requirement = inputs[type];
                requirement.shortFall = requirement.quantity;
                inputs[type] = requirement;
            }
            
        }
    }
}