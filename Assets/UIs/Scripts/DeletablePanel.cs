using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;


public class DeletablePanel : MonoBehaviour
{
    public void Delete()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);

        var expandablePanel = GetComponentInParent<ExpandablePanel>();
        if (expandablePanel != null)
            expandablePanel.Refresh();
    }
}