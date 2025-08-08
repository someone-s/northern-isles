using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PortCargoDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textArea;
    public UnityEvent OnOutboundChange;

    public void OnOutboundChanged(IReadOnlyCollection<CargoType> cargos)
    {
        OnOutboundReset(cargos);

        OnOutboundChange.Invoke();
    }

    public void OnOutboundReset(IReadOnlyCollection<CargoType> cargos)
    {
        string newText = string.Empty;
        foreach (var cargo in cargos)
            newText += SpriteCargo.Instance.Texts[cargo];
        textArea.text = newText;
    }
}
