using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn.Unity.Attributes;

public class Region : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private DialogueRunner runner;
    [SerializeField] private YarnProject project;
    [YarnNode(nameof(project))] public string startNode;

    private JToken cachedState;
    [SerializeField] private bool shown = true;

    private void Awake()
    {
        button.onClick.AddListener(Display);

        button.gameObject.SetActive(shown);
    }

    public void Display()
    {
        if (shown == false)
            return;
            
        shown = false;

        RegionVisual.Instance.Exit();

        runner.Stop();
        runner.SetProject(project);
        runner.StartDialogue(startNode);

        void listener()
        {
            StateTrack.Instance.SaveQuickState();
            runner.onDialogueComplete.RemoveListener(listener);
        }
        runner.onDialogueComplete.AddListener(listener);

        button.gameObject.SetActive(false);
    }

    public JToken GetState()
    {
        cachedState = JToken.FromObject(shown);
        return cachedState;
    }

    public void SetState(JToken json)
    {
        cachedState = json;
        shown = cachedState.ToObject<bool>();

        button.gameObject.SetActive(shown);
    }

    public void Rollback()
    {
        SetState(cachedState);
    }
}
