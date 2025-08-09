using UnityEngine;

public class BuildingScaleVisual : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private float durationS;
    private float elapsedS;

    private void Start()
    {
        transform.localScale = Vector3.zero;
        elapsedS = 0f;
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
}
