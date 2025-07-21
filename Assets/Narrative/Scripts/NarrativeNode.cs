using System.Collections.Generic;
using UnityEngine;

public class NarrativeNode : MonoBehaviour
{
    [SerializeField] private bool complete = false;
    [SerializeField] private bool stepByStep = false;

    private NarrativeNode parent;
    private List<NarrativeNode> children;

    private void Awake()
    {
        if (transform.parent != null)
        {
            if (transform.parent.TryGetComponent<NarrativeNode>(out var node))
                parent = node;
        }

        children = new();
        foreach (Transform childTransform in transform)
        {
            if (childTransform.TryGetComponent<NarrativeNode>(out var node))
                children.Add(node);
        }

        if (stepByStep)
        {
            for (int i = 1; i < children.Count; i++)
                children[i].gameObject.SetActive(false);
        }
    }

    public void MarkComplete()
    {
        complete = true;

        if (parent != null)
            parent.Evaluate();
    }

    private void Evaluate()
    {
        if (stepByStep)
        {
            for (int i = 1; i < children.Count; i++)
                if (children[i - 1].gameObject.activeSelf && !children[i].gameObject.activeSelf)
                {
                    children[i].gameObject.SetActive(true);
                    break;
                }
        }

        foreach (var child in children)
            if (!child.complete)
                return;

        complete = true;
        
        if (parent != null)
            parent.Evaluate();
    }
}
