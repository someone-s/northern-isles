using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(VerticalLayoutGroup))]
[RequireComponent(typeof(ContentSizeFitter))]
public class ExpandablePanel : RefreshablePanel
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
}
