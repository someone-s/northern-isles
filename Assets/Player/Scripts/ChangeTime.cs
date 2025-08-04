using Sirenix.OdinInspector;
using UnityEngine;

public class ChangeTime : MonoBehaviour
{
    [Button()]
    public void SetSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}
