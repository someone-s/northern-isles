using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class SpawnPanel : MonoBehaviour
{
    public GameObject prefab;

    [ProButton]
    private void Spawn()
    {
        Instantiate(prefab, transform);

        GetComponent<ExpandablePanel>().Refresh();
    }
}
