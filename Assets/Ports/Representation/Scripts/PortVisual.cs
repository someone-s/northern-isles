using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasSwap))]
public class PortVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text textArea;
    public string Name => textArea.text;
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private float durationS;

    private CanvasSwap canvasSwap;
    private float elapsedS;
    private JToken cachedState;

    private void Awake()
    {
        canvasSwap = GetComponent<CanvasSwap>();
        transform.localScale = Vector3.zero;
        elapsedS = 0f;
        enabled = false;
    }


    private void Update()
    {
        elapsedS += Time.deltaTime;
        if (elapsedS < durationS)
        {
            float newScale = curve.Evaluate(elapsedS / durationS);
            transform.localScale = new(newScale, newScale, newScale);
        }
        else
        {
            transform.localScale = Vector3.one;
            enabled = false;
        }
    }

    public void SetIconMode(IconMode mode)
    {
        switch (mode)
        {
            case IconMode.Dynamic:
                canvasSwap.Begin();
                break;

            case IconMode.Static:
                canvasSwap.End();
                break;
        }
    }


    public enum IconMode
    {
        Dynamic,
        Static
    }

    public JToken GetState()
    {
        cachedState = JToken.FromObject(new VisualState()
        {
            enabled = enabled,
            elapsedS = elapsedS,
            scale = transform.localScale.x
        });
        return cachedState;
    }

    public void SetState(JToken json)
    {
        cachedState = json;
        var state = cachedState.ToObject<VisualState>();
        transform.localScale = new(state.scale, state.scale, state.scale);
        elapsedS = state.elapsedS;
        enabled = state.enabled;
    }

    public void Rollback()
    {
        SetState(cachedState);
    }

    private struct VisualState
    {
        public bool enabled;
        public float elapsedS;
        public float scale;
    }
}