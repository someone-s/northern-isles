using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextReference : MonoBehaviour
{
    public TextEntry Entry { get; private set; }
    private TMP_Text textArea;

    private void Awake()
    {
        textArea = GetComponent<TMP_Text>();
    }

    public void SetEntry(TextEntry entry)
    {
        Entry = entry;

        Refresh();
    }

    public void Refresh()
    {
        textArea.text = Entry.text;

        if (TryGetComponent<RefreshablePanel>(out var panel))
            panel.Refresh();
    }

    public void BeginCollapse()
    {
        IEnumerator routine = Collapse();
        StartCoroutine(routine);
    }

    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    private IEnumerator Collapse()
    {

        float time = 0f;
        while (transform.localScale.y > 0f)
        {
            time += Time.deltaTime;
            var s = transform.localScale;
            s.y = curve.Evaluate(time);
            transform.localScale = s;

            if (TryGetComponent<RefreshablePanel>(out var panel))
                panel.Refresh();

            yield return null;
        }

        CollapseInstant();
    }

    public void CollapseInstant()
    {

        gameObject.SetActive(false);
        transform.SetParent(null);
        Destroy(gameObject);
    }
}