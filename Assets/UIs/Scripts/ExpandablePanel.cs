using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ExpandablePanel : MonoBehaviour
{
    [SerializeField] private float expandHeight;
    [SerializeField] private float collapseHeight;

    [SerializeField] private bool state = false;
    public UnityEvent OnBeginExpand;
    public UnityEvent OnEndExpand;
    public UnityEvent OnBeginCollapse;
    public UnityEvent OnEndCollapse;

    public void Toggle()
    {
        if (state)
        {
            OnBeginCollapse.Invoke();
            var rt = transform as RectTransform;
            rt.sizeDelta = new(rt.sizeDelta.x, collapseHeight);
            OnEndCollapse.Invoke();
        }
        else
        {
            OnBeginExpand.Invoke();
            var rt = transform as RectTransform;
            rt.sizeDelta = new(rt.sizeDelta.x, expandHeight);
            OnEndExpand.Invoke();
        }

        var layout = GetComponentInParent<VerticalLayoutGroup>();
        if (layout != null)
            layout.SetLayoutVertical();

        var fitter = GetComponentInParent<ContentSizeFitter>();
        if (fitter != null)
            LayoutRebuilder.MarkLayoutForRebuild(fitter.transform as RectTransform);
        state = !state;
    }
}
