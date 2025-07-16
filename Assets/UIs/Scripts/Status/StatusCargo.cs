using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatusCargo : MonoBehaviour
{
    [SerializeField] private TMP_SpriteAsset spriteAsset;

    private Dictionary<CargoType, Sprite> sprites;
    public ReadOnlyDictionary<CargoType, Sprite> Sprites { get; private set; }

    private void Awake()
    {
        sprites =
            Enum.GetValues(typeof(CargoType))
            .Cast<CargoType>()
            .Select(type => (
                type,
                spriteAsset.spriteGlyphTable[spriteAsset.GetSpriteIndexFromHashcode(TMP_TextUtilities.GetHashCode(Enum.GetName(typeof(CargoType), type)))].sprite
            ))
            .ToDictionary(keySelector: tuple => tuple.type, elementSelector: tuple => tuple.sprite);
        Sprites = new(sprites);
    }

}