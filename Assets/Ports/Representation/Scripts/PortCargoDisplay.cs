using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PortCargoDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textArea;

    public void OnOutboundChanged(IReadOnlyCollection<CargoType> cargos)
    {
        string newText = string.Empty;
        foreach (var cargo in cargos)
            newText += SpriteCargo.Instance.Texts[cargo];
        textArea.text = newText;
    }
}
