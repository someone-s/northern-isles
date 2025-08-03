using UnityEngine;

public class GeneratorVisual : MonoBehaviour
{
    public void OnProgressUpdate(float progress)
    {
        var t = transform.localScale;
        t.x = progress;
        transform.localScale = t;
    }
}