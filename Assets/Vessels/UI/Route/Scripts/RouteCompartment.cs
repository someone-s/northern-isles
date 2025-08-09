using System.Collections.Generic;
using UnityEngine;

public class RouteCompartment : MonoBehaviour
{    [SerializeField] private Transform content;
    [SerializeField] private GameObject cargoPrefab;

    public RouteDisplay Display { get; private set; }

    private List<RouteLoad> loads;

    private void Awake()
    {
        loads = new();
    }

    public void SetDisplay(RouteDisplay display)
    {
        Display = display;
    }

    public void SetStorage(VesselStorage storage)
    {
        storage.OnStorageChange.AddListener(Generate);
    }

    public void RemoveStorage(VesselStorage storage)
    {
        storage.OnStorageChange.RemoveListener(Generate);
    }

    public void Generate(IReadOnlyCollection<CargoType> cargos, float Capacity)
    {
        foreach (RouteLoad load in loads)
        {
            load.gameObject.SetActive(false);
            Destroy(load.gameObject);
        }
        loads.Clear();

        foreach (CargoType cargo in cargos)
        {
            var cargoObject = Instantiate(cargoPrefab, content);
            var load = cargoObject.GetComponent<RouteLoad>();
            load.SetCompartment(this);
            load.SetCargo(cargo);
            loads.Add(load);
        }

        RefreshLayout();
    }

    public void DeleteLoad(RouteLoad load)
    {
        Display.DeleteLoad(load.Cargo);
        loads.Remove(load);
        load.gameObject.SetActive(false);
        Destroy(load.gameObject);

        RefreshLayout();
    }

    private void RefreshLayout()
    {
        content.GetComponent<OnDemandHorizontalLayout>().Refresh();
    }
}