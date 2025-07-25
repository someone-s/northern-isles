using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PortCargoDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textArea;
    private HashSet<CargoType> presentCargos;

    private void Awake()
    {
        presentCargos = new();
    }

    public void OnCargoPresented(CargoType type, float total)
    {
        if (total <= 0f)
            return;

        if (presentCargos.Add(type))
            enabled = true;
    }

    public void OnCargoVaccated(CargoType type, float total)
    {
        if (total > 0f)
            return;

        if (presentCargos.Remove(type))
            enabled = true;
    }

    private void LateUpdate()
    {
        string newText = string.Empty;
        foreach (var cargo in presentCargos)
            newText += SpriteCargo.Instance.Texts[cargo];
        textArea.text = newText;

        enabled = false;
    }
}
