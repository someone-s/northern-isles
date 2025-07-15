using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class SpawnPanel : MonoBehaviour
{

    public GameObject SpawnNewPanel(GameObject prefab, int offset = 0)
    {
        GameObject newPanel = Instantiate(prefab, transform);
        newPanel.transform.SetSiblingIndex(Mathf.Clamp(transform.childCount - 1 - offset, 0, transform.childCount - 1));

        GetComponent<ExpandablePanel>().Refresh();

        return newPanel;
    }
}
