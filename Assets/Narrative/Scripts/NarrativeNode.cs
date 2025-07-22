using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NarrativeNode : MonoBehaviour
{
    [SerializeField] private bool complete = false;
    [SerializeField] private bool stepByStep = false;

    private NarrativeNode parent;
    private List<NarrativeNode> children;

    public UnityEvent OnBeginStart;
    public UnityEvent OnBeginTail;
    public UnityEvent OnComplete;

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

        if (parent != null)
            gameObject.SetActive(false);
    }

    private void Start()
    {
        if (parent == null)
            Begin();
    }

    private void Begin()
    {
        OnBeginStart.Invoke();

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (stepByStep)
        {
            if (children.Count >= 1)
                children[0].Begin();
        }
        else
        {
            foreach (var child in children)
                child.Begin();
        }

        OnBeginTail.Invoke();

        ProcessComplete();
    }

    public void MarkComplete()
    {
        complete = true;

        if (!gameObject.activeSelf)
            return;

        ProcessComplete();
    }
    
    private void ProcessComplete()
    {
        if (!complete)
            return;

        OnComplete.Invoke();

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
                    children[i].Begin();
                    break;
                }
        }

        foreach (var child in children)
            if (!child.complete)
                return;

        complete = true;
        OnComplete.Invoke();
        
        if (parent != null)
            parent.Evaluate();
    }
}
