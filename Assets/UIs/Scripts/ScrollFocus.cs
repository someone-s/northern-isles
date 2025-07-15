using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollFocus : MonoBehaviour
{
    private ScrollRect scrollRect;
    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    [SerializeField] private bool x = true;
    [SerializeField] private bool y = true;

    [ProButton]
    public void GoTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        var newPosition = (transform as RectTransform).InverseTransformPoint(scrollRect.content.position) - (transform as RectTransform).InverseTransformPoint(target.position);
        if (!x)
            newPosition.x = scrollRect.content.anchoredPosition.x;
        if (!y)
            newPosition.y = scrollRect.content.anchoredPosition.y;

        scrollRect.content.anchoredPosition = newPosition;
    }
}
