using System.Collections.Generic;
using UnityEngine;

public class PortAnchor : MonoBehaviour
{
    [SerializeField] private List<Transform> anchors;

    public Vector3 GetAccesoryPosition(int index)
    {
        return anchors[index].position;
    }
}