using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class ChangeTime : MonoBehaviour
{
    [ProButton]
    public void SetSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}
