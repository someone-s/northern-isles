using System;
using System.Collections.Generic;
using TMPro;
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
                // reference.gameObject.SetActive(false);
                // reference.transform.SetParent(null);
                // Destroy(reference.gameObject);

                // if (TryGetComponent<RefreshablePanel>(out var panel))
                //     panel.Refresh();

                break;
            }
        }
    }

}
