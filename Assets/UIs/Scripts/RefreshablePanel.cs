using UnityEngine;
using UnityEngine.UI;

public class RefreshablePanel : MonoBehaviour
{
    public void Refresh()
    {
        RefreshSelfLayout();
        RefreshParentLayout();
    }

    protected void RefreshSelfLayout()
    {
        if (TryGetComponent<VerticalLayoutGroup>(out var layout))
            layout.SetLayoutVertical();

        if (TryGetComponent<ContentSizeFitter>(out var groupFitter))
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
    protected void RefreshParentLayout()
    {
        var parentPanel = transform.parent.GetComponentInParent<RefreshablePanel>();
        if (parentPanel != null)
            parentPanel.Refresh();
        else
        {
            var layout = transform.parent.GetComponentInParent<VerticalLayoutGroup>();
            if (layout != null)
                layout.SetLayoutVertical();

            var groupFitter = transform.parent.GetComponentInParent<ContentSizeFitter>();
            if (groupFitter != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(groupFitter.transform as RectTransform);
        }
    }
}
