using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VesselIcon : MonoBehaviour
{
    [SerializeField] private TMP_Text textArea;

    public void OnStorageChanged(IReadOnlyCollection<CargoType> cargos, float target)
    {
        string newText = string.Empty;
        var iter = cargos.GetEnumerator();
        for (int i = 0; i < target; i++)
        {
            if (i < cargos.Count)
            {
                newText += SpriteCargo.Instance.Texts[iter.Current];
                iter.MoveNext();
            }
            else
            {
                newText += SpriteCargo.Instance.EmptyText;
            }
        }
        iter.Dispose();
        textArea.text = newText;
    }
}
