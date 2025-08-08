using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class NarrativeOrientationDatabase : MonoBehaviour
{
    public static NarrativeOrientationDatabase Instance { get; private set; }

    private NarrativeOrientationDatabase()
    {
        Instance = this;
    }

    private Dictionary<string, Transform> lookups;
    public ReadOnlyDictionary<string, Transform> Lookups;

    private void Awake()
    {
        lookups = new();

        foreach (Transform child in transform)
            lookups[child.name] = child;

        Lookups = new(lookups);
    }
}