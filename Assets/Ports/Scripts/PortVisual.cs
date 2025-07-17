using TMPro;
using UnityEngine;

public class PortVisual : MonoBehaviour
{
    [SerializeField] private GameObject iconStatic;
    [SerializeField] private GameObject iconDynamic;
    [SerializeField] private TMP_Text textArea;
    public string Name => textArea.text;

    public void SetIconMode(IconMode mode)
    {
        switch (mode)
        {
            case IconMode.Dynamic:
                iconStatic.SetActive(false);
                iconDynamic.SetActive(true);
                break;

            case IconMode.Static:
                iconStatic.SetActive(true);
                iconDynamic.SetActive(false);
                break;
        }
    }

    public enum IconMode
    {
        Dynamic,
        Static
    }
}