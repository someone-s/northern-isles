using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Attributes;

public class Narrative : MonoBehaviour, IStateProvider
{
    [SerializeField] private DialogueRunner runner;
    [SerializeField] private YarnProject project;

    [SerializeField] private int index = 0;
    [SerializeField] private List<Chapter> chapters;

    private bool settingState;
    private JToken cachedState;

    [Serializable]
    private class Chapter
    {
        [YarnNode(nameof(project))] public string startNode;
    }

    private void Start()
    {
        settingState = false;

        StateTrack.Instance.AddProvider(this);
        StateTrack.Instance.SaveState();

        runner.onDialogueComplete.AddListener(OnDiaglougeStopped);

        runner.SetProject(project);
        runner.StartDialogue(chapters[index].startNode);
        enabled = false;
    }

    private void OnDiaglougeStopped()
    {
        if (settingState)
            return;

        index++;
        StateTrack.Instance.SaveState();
        enabled = true;
    }

    public void Update()
    {
        if (index < chapters.Count)
            runner.StartDialogue(chapters[index].startNode);
        enabled = false;
    }

    public string GetName() => "Narrative";

    public int GetPriority() => 5;

    public JToken GetState()
    {
        cachedState = JToken.FromObject(index);
        return cachedState;
    }

    public void SetState(JToken json)
    {
        settingState = true;

        cachedState = json;
        index = cachedState.ToObject<int>();

        runner.Stop();
        runner.StartDialogue(chapters[index].startNode);
        settingState = false;
    }

    public void Rollback()
    {
        SetState(cachedState);
    }
    
}