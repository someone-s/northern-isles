
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Building : MonoBehaviour
{
    private Generator[] generators;

    public Port Port { get; private set; }

    [field: SerializeField]
    public BuildingType Type { get; private set; }

    private void Awake()
    {
        generators = GetComponents<Generator>();
    }

    public void Setup(Port port, int position = 0)
    {
        Port = port;
        foreach (var generator in generators)
            generator.SetPort(Port);
        transform.position = Port.Anchor.GetAccesoryPosition(position);
    }

    public string GetState()
    {
        return JsonUtility.ToJson(new BuidlingState()
        {
            generatorState = generators.Select(generator => generator.GetState()).ToList()
        });
    }

    public void SetState(string json)
    {
        var state = JsonUtility.FromJson<BuidlingState>(json);
        for (int i = 0; i < generators.Length; i++)
            generators[i].SetState(state.generatorState[i]);
    }

    public void Rollback()
    {
        foreach (var generator in generators)
            generator.Rollback();
    }

    [Serializable]
    private struct BuidlingState
    {
        public List<string> generatorState;
    }
}

[Serializable]
public enum BuildingType
{
    CattleFarm,
    SheepFarm,
    CoalYard,
    Butcher,
    TextileMill,
    Quarry,
    Mountain,
    Brewery,
    FlourMill,
    GeneralStore,
    EngineeringShop,
    HotelPub,
    Bakery,
    ConstructionSite,
    PostOffice,
    House
}