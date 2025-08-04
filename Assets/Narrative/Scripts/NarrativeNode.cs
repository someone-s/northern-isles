using System.Collections.Generic;
using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

public class NarrativeNode : MonoBehaviour, IStateProvider
{
    [SerializeField] private bool complete = false;
    [SerializeField] private bool stepByStep = false;

    private NarrativeNode parent;
    private List<NarrativeNode> children;
    private bool cachedState;

    public UnityEvent OnReset;
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

        if (parent == null)
            StateTrack.Instance.AddProvider(this);
        else
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

    [ProButton]
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

    public string GetName() => "NarrativeNode";

    public int GetPriority() => 3;

    public JToken GetState()
    {
        cachedState = complete;
        return JToken.FromObject(new NodeState()
        {
            complete = cachedState,
            nested = children.Select(child => child.GetState()).ToList()
        });
    }

    public void SetState(JToken json)
    {
        SetStateNested(json);

        Begin();
    }

    private void Rebegin()
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
                child.Rebegin();
        }

        OnBeginTail.Invoke();

        ProcessComplete();
    }

    public void SetStateNested(JToken json)
    {
        var state = json.ToObject<NodeState>();
        cachedState = state.complete;
        complete = cachedState;

        for (int i = 0; i < children.Count; i++)
            children[i].SetState(state.nested[i]);

        OnReset.Invoke();
    }

    public void Rollback()
    {
        complete = cachedState;
        foreach (var child in children)
            child.Rollback();
    }

    private struct NodeState
    {
        public bool complete;
        public List<JToken> nested;
    }
}
