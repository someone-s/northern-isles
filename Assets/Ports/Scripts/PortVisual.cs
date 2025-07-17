using UnityEngine;

public class PortVisual : MonoBehaviour
{
    [SerializeField] private GameObject iconStatic;
    [SerializeField] private GameObject iconDynamic;

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