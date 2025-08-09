using UnityEngine;

public class BuildingDeliveryVisual : MonoBehaviour
{
    public void OnCountdownUpdate(float normalizedValue)
    {
        var t = transform.localScale;
        t.y = normalizedValue;
        transform.localScale = t;
    }
}