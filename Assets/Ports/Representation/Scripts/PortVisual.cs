using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasSwap))]
public class PortVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text textArea;
    public string Name => textArea.text;

    private CanvasSwap canvasSwap;

    private void Start()
    {
        canvasSwap = GetComponent<CanvasSwap>();
    }

    public void SetIconMode(IconMode mode)
    {
        switch (mode)
        {
            case IconMode.Dynamic:
                canvasSwap.Begin();
                break;

            case IconMode.Static:
                canvasSwap.End();
                break;
        }
    }


    public enum IconMode
    {
        Dynamic,
        Static
    }
}