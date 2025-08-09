
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Guid Guid { get; private set; }

    private BuildingGenerator[] generators;

    public Port Port { get; private set; }

    [field: SerializeField]
    public BuildingType Type { get; private set; }

    public int Position { get; private set; }

    private bool deliverySatisfied;

    private void Awake()
    {
        generators = GetComponents<BuildingGenerator>();
        deliverySatisfied = false;
    }

    public void Setup(Port port, Guid guid, int position = 0)
    {
        Port = port;
        Guid = guid;
        Position = position;
        foreach (var generator in generators)
            generator.Setup(Port);
        transform.position = Port.Anchor.GetAccesoryPosition(position);
    }

    public void NotifyDeliverySatisfied()
    {
        deliverySatisfied = true;

        if (BuildingDatabase.Instance.BuildingNeighbours[Port].All(building => building.deliverySatisfied))
            Port.Visual.SetPass();
    }

    public void NotifyDeliveryDissatisfied()
    {
        deliverySatisfied = false;

        Port.Visual.SetFail();
    }

    public void SetState(JToken json)
    {
        var state = json.ToObject<BuidlingState>();
        for (int i = 0; i < generators.Length; i++)
            generators[i].SetState(state.generatorState[i]);
    }

    public JToken GetState()
    {
        return JToken.FromObject(new BuidlingState()
        {
            generatorState = generators.Select(generator => generator.GetState()).ToList()
        });
    }

    public void Rollback()
    {
        foreach (var generator in generators)
            generator.Rollback();
    }

    [Serializable]
    private struct BuidlingState
    {
        public List<JToken> generatorState;
    }
}

[Serializable]
public enum BuildingType
{
    GrainFarm,
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