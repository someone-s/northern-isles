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
    private bool shown = true;

    private void Awake()
    {
        button.onClick.AddListener(Display);
    }

    public void Display()
    {
        shown = false;

        RegionVisual.Instance.Exit();

        runner.Stop();
        runner.SetProject(project);
        runner.StartDialogue(startNode);

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
