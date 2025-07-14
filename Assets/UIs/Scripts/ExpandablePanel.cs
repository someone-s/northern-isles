using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(VerticalLayoutGroup))]
[RequireComponent(typeof(ContentSizeFitter))]
public class ExpandablePanel : MonoBehaviour
{
    [SerializeField] private bool state = false;
    public UnityEvent OnBeginExpand;
    public UnityEvent OnEndExpand;
    public UnityEvent OnBeginCollapse;
    public UnityEvent OnEndCollapse;

    private void Start()
    {
        ApplyState();
        RefreshParentLayout();
    }

    public void Refresh()
    {
        RefreshSelfLayout();
        RefreshParentLayout();
    }

    public void Toggle()
    {
        state = !state;

        ApplyState();
        RefreshParentLayout();
    }

    private void ApplyState()
    {
        if (state)
            OnBeginExpand.Invoke();
        else
            OnBeginCollapse.Invoke();

        RefreshSelfLayout();

        if (state)
            OnEndExpand.Invoke();
        else
            OnEndCollapse.Invoke();
    }

    private void RefreshSelfLayout()
    {
        GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
    private void RefreshParentLayout()
    {
        var parentPanel = transform.parent.GetComponentInParent<ExpandablePanel>();
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
