using UnityEngine;

public class ReorderablePanel : MonoBehaviour
{
    public void MoveUp()
    {
        int currentIndex = transform.GetSiblingIndex();
        int newIndex = Mathf.Clamp(currentIndex - 1, 0, transform.parent.childCount - 1);
        transform.SetSiblingIndex(newIndex);

        var focus = GetComponentInParent<ScrollFocus>();
        if (focus != null)
            focus.GoTo(transform as RectTransform);
    }

    public void MoveDown()
    {
        int currentIndex = transform.GetSiblingIndex();
        int newIndex = Mathf.Clamp(currentIndex + 1, 0, transform.parent.childCount - 1);
        transform.SetSiblingIndex(newIndex);

        var focus = GetComponentInParent<ScrollFocus>();
        if (focus != null)
            focus.GoTo(transform as RectTransform);
    }
}
