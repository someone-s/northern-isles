using UnityEngine;
using UnityEngine.UI;

public class OnDemandHorizontalLayout : MonoBehaviour
{
    public void Refresh()
    {
        var rt = transform as RectTransform;

        float x = 0f;
        for (int i = 0; i < rt.childCount; i++)
        {
            var ct = rt.GetChild(i);

            if (!ct.gameObject.activeSelf)
                continue;

            if (ct.TryGetComponent(out LayoutElement e) && e.ignoreLayout == true)
                continue;

            var crt = ct as RectTransform;
            var delta = crt.sizeDelta.x;

            crt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, x, delta);

            x += delta;
        }

        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
    }
}