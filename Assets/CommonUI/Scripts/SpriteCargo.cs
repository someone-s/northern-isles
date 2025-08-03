using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using UnityEngine;

public class SpriteCargo : MonoBehaviour
{
    [SerializeField] private TMP_SpriteAsset spriteAsset;

    public CargoType[] Cargos { get; private set; }
    public string[] Names { get; private set; }
    public List<TMP_Dropdown.OptionData> Options { get; private set; }
    private Dictionary<CargoType, int> indices;
    public ReadOnlyDictionary<CargoType, int> Indicies { get; private set; }

    private Dictionary<CargoType, string> texts;
    public ReadOnlyDictionary<CargoType, string> Texts { get; private set; }
    public string EmptyText { get; private set; }

    private Dictionary<CargoType, Sprite> sprites;
    public ReadOnlyDictionary<CargoType, Sprite> Sprites { get; private set; }

    public static SpriteCargo Instance { get; private set; } = null;
    private SpriteCargo()
    {
        Instance = this;
    }

    private void Awake()
    {
         string cargoSpritePrefix = $"<sprite=\"{spriteAsset.name}\" name=\"";
         string cargoSpriteSuffix = "\" tint=1>";
        Func<string, string> getCargoSpriteName = (string name) => $"{cargoSpritePrefix}{name}{cargoSpriteSuffix}";

        Names = Enum.GetNames(typeof(CargoType));
        Options = Names.Select(name => new TMP_Dropdown.OptionData(getCargoSpriteName(name))).ToList();

        Cargos = Enum.GetValues(typeof(CargoType)).Cast<CargoType>().ToArray();
        indices = new();
        int i = 0;
        foreach (var type in Cargos)
        {
            indices.Add(type, i);
            i++;
        }
        Indicies = new(indices);

        texts =
            Cargos
            .Select(type => (
                type: type,
                text: getCargoSpriteName(Enum.GetName(typeof(CargoType), type))
            ))
            .ToDictionary(keySelector: tuple => tuple.type, elementSelector: tuple => tuple.text);
        Texts = new(texts);

        EmptyText = getCargoSpriteName("None");

        sprites =
            Cargos
            .Select(type => (
                type: type,
                sprite: spriteAsset.spriteGlyphTable[spriteAsset.GetSpriteIndexFromHashcode(TMP_TextUtilities.GetHashCode(Enum.GetName(typeof(CargoType), type)))].sprite
            ))
            .ToDictionary(keySelector: tuple => tuple.type, elementSelector: tuple => tuple.sprite);
        Sprites = new(sprites);
    }
}