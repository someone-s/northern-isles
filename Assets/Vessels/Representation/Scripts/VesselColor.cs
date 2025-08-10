using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VesselColor : MonoBehaviour
{
    [SerializeField] private Color satisfiedColor;
    [SerializeField] private Color dissatisfiedColor;

    [SerializeField] private Image dynamicVisual;
    [SerializeField] private Renderer staticVisual;
    private Material staticMaterial;
    [SerializeField] private Renderer lineVisual;
    private Material lineMaterial;

    private bool isSatisfied;
    private JToken cachedState;

    private void Awake()
    {
        staticMaterial = staticVisual.material;
        lineMaterial = lineVisual.material;
        SetSatified();
    }

    public void SetSatified()
    {
        dynamicVisual.color = satisfiedColor;
        staticMaterial.color = satisfiedColor;
        lineMaterial.color = satisfiedColor;
        isSatisfied = true;
    }

    public void SetDissatified()
    {
        dynamicVisual.color = dissatisfiedColor;
        staticMaterial.color = dissatisfiedColor;
        lineMaterial.color = dissatisfiedColor;
        isSatisfied = false;
    }

    public JToken GetState()
    {
        cachedState = JToken.FromObject(isSatisfied);
        return cachedState;
    }

    public void SetState(JToken json)
    {
        cachedState = json;

        var state = cachedState.ToObject<bool>();
        if (state)
            SetSatified();
        else
            SetDissatified();
    }

    public void Rollback()
    {
        SetState(cachedState);
    }
}