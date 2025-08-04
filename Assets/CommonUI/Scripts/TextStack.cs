using UnityEngine;

public class TextStack : MonoBehaviour
{

    [SerializeField] private GameObject textPrefab;


    public void AddLine(TextEntry entry)
    {
        var textObject = Instantiate(textPrefab, transform);

        var textRef = textObject.GetComponent<TextReference>();
        textRef.SetEntry(entry);
    }

    public void InsertLine(TextEntry nest, TextEntry entry)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var reference = transform.GetChild(i).GetComponent<TextReference>();
            if (reference.Entry == nest)
            {
                var textObject = Instantiate(textPrefab, transform);
                textObject.transform.SetSiblingIndex(i + 1);

                var textRef = textObject.GetComponent<TextReference>();
                textRef.SetEntry(entry);

                break;
            }
        }
    }

    public void UpdateLine(TextEntry entry)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var reference = transform.GetChild(i).GetComponent<TextReference>();
            if (reference.Entry == entry)
            {
                reference.Refresh();

                break;
            }
        }
    }

    public void Remove(TextEntry entry)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var reference = transform.GetChild(i).GetComponent<TextReference>();
            if (reference.Entry == entry)
            {
                reference.BeginCollapse();

                break;
            }
        }
    }

    public void RemoveInstant(TextEntry entry)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var reference = transform.GetChild(i).GetComponent<TextReference>();
            if (reference.Entry == entry)
            {
                reference.CollapseInstant();

                break;
            }
        }
    }

}
