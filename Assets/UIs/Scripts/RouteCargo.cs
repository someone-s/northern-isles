using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouteCargo : MonoBehaviour
{
    [SerializeField] private string cargoSpritePrefix = "<sprite=\"IconSmall\" name=\"";
    [SerializeField] private string cargoSpriteSuffix = "\">";
    private string GetCargoSprite(string name) => $"{cargoSpritePrefix}{name}{cargoSpriteSuffix}";

    public CargoType[] Cargos { get; private set; }
    public string[] Names { get; private set; }
    public List<TMP_Dropdown.OptionData> Options { get; private set; }
    private Dictionary<CargoType, int> indices;
    public ReadOnlyDictionary<CargoType, int> Indicies { get; private set; }

    private void Awake()
    {
        Names = Enum.GetNames(typeof(CargoType));
        Options = Names.Select(name => new TMP_Dropdown.OptionData(GetCargoSprite(name))).ToList();

        Cargos = Enum.GetValues(typeof(CargoType)).Cast<CargoType>().ToArray();
        indices = new();
        int i = 0;
        foreach (var type in Cargos)
        {
            indices.Add(type, i);
            i++;
        }
        Indicies = new(indices);
    }
}