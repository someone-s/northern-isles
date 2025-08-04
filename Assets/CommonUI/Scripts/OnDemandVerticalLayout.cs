using UnityEngine;
using UnityEngine.UI;

public class OnDemandVerticalLayout : MonoBehaviour
{
    public void Refresh()
    {
        var rt = transform as RectTransform;

        float y = 0f;
        for (int i = 0; i < rt.childCount; i++)
        {
            var ct = rt.GetChild(i);

            if (!ct.gameObject.activeSelf)
                continue;

            if (ct.TryGetComponent(out LayoutElement e) && e.ignoreLayout == true)
                continue;

            var crt = ct as RectTransform;
            var delta = crt.sizeDelta.y * crt.localScale.y;

            crt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, y, delta);

            y += delta;
        }

        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
    }
}