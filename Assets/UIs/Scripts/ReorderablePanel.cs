using UnityEngine;
using UnityEngine.Events;

public class ReorderablePanel : MonoBehaviour
{

    public UnityEvent<int, int> OnMove;

    private void MoveTo(int index)
    {
        int oldIndex = transform.GetSiblingIndex();
        int newIndex = Mathf.Clamp(index, 0, transform.parent.childCount - 1);
        transform.SetSiblingIndex(newIndex);

        var focus = GetComponentInParent<ScrollFocus>();
        if (focus != null)
            focus.GoTo(transform as RectTransform);

        OnMove.Invoke(oldIndex, newIndex);
    }

    public void MoveUp()
    {
        MoveTo(transform.GetSiblingIndex() - 1);
    }

    public void MoveDown()
    {
        MoveTo(transform.GetSiblingIndex() + 1);
    }
}
