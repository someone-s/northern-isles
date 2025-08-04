using UnityEngine;
using Yarn.Unity;

public class NarrativeModifyParameter : MonoBehaviour
{
    [SerializeField] private string variableName;
    [SerializeField] private bool boolValue;

    public void Apply()
    {
        var variable = FindFirstObjectByType<DialogueRunner>().VariableStorage;

        variable.SetValue(variableName, boolValue);
    }
}
