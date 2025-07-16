using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusCompartment : MonoBehaviour
{
    [SerializeField] private TMP_Text textArea;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject cargoPrefab;

    public StatusData Data { get; private set; }
    public VesselCompartment Compartment { get; private set; }

    private Dictionary<CargoType, StatusLoad> cargos;

    public void SetData(StatusData data)
    {
        Data = data;
    }

    public void SetCompartment(VesselCompartment compartment)
    {
        Compartment = compartment;

        textArea.text = Data.Vessel.Compartments.IndexOf(Compartment).ToString();

        cargos = new();
        foreach (var entry in Compartment.Cargos)
            if (entry.Value > 0f)
                CreateLoad(entry.Key);
        RefreshLayout();
        

        Compartment.OnLoadCargo.AddListener(OnLoadCargo);
        Compartment.OnUnloadCargo.AddListener(OnUnloadCargo);
    }

    private void CreateLoad(CargoType type)
    {
        var cargoObject = Instantiate(cargoPrefab, content);
        var load = cargoObject.GetComponent<StatusLoad>();
        load.SetCompartment(this);
        load.SetLoad(type);
        cargos.Add(type, load);
    }

    private void RemoveLoad(CargoType type)
    {
        var load = cargos[type];
        load.gameObject.SetActive(false);
        Destroy(load.gameObject);
        cargos.Remove(type);
    }

    private void RefreshLayout()
    {
        content.GetComponent<HorizontalLayoutGroup>().SetLayoutVertical();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.transform as RectTransform);
    }

    private void OnLoadCargo(CargoType type, float quantity, float total)
    {
        if (total > 0f && !cargos.ContainsKey(type))
        {
            CreateLoad(type);
            RefreshLayout();
        }
    }
    private void OnUnloadCargo(CargoType type, float quantity, float total)
    {
        if (total >= 0f && cargos.ContainsKey(type))
        {
            RemoveLoad(type);
            RefreshLayout();
        }

    }
}