using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class SpawnPanel : MonoBehaviour
{
    public GameObject prefab;
    public int offset = 1;

    [ProButton]
    public void Spawn()
    {
        GameObject newPanel = Instantiate(prefab, transform);
        newPanel.transform.SetSiblingIndex(Mathf.Clamp(transform.childCount - 1 - offset, 0, transform.childCount - 1));

        GetComponent<ExpandablePanel>().Refresh();
    }
}
