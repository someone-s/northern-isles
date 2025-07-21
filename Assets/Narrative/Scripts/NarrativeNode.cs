using System.Collections.Generic;
using UnityEngine;

public class NarrativeNode : MonoBehaviour
{
    [SerializeField] private bool complete = false;

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
    }

    public void MarkComplete()
    {
        complete = true;

        if (parent != null)
            parent.Evaluate();
    }

    private void Evaluate()
    {
        foreach (var child in children)
            if (!child.complete)
                return;

        complete = true;
    }
}
