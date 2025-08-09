using UnityEngine;

public class BuildingGeneratorVisual : MonoBehaviour
{
    public void OnProgressUpdate(float progress)
    {
        var t = transform.localScale;
        t.x = progress;
        transform.localScale = t;
    }
}