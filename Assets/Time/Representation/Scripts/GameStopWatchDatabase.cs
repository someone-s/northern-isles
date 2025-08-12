using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStopWatchDatabase : MonoBehaviour
{
    public static GameStopWatchDatabase Instance { get; private set; }

    [SerializeField] private GameObject stopWatchPrefab;

    private GameStopWatchDatabase()
    {
        Instance = this;
    }

    private Dictionary<Guid, GameStopWatch> stopWatches = new();
    public IReadOnlyDictionary<Guid, GameStopWatch> StopWatches => stopWatches;

    public Guid CreateNewStopWatch()
    {
        GameObject stopWatchObject = Instantiate(stopWatchPrefab, transform);
        var stopWatch = stopWatchObject.GetComponent<GameStopWatch>();
        var guid = Guid.NewGuid();
        stopWatches.Add(guid, stopWatch);
        return guid;
    }

    public void DeleteStopWatch(Guid guid)
    {
        if (stopWatches.TryGetValue(guid, out GameStopWatch stopWatch))
        {
            stopWatch.gameObject.SetActive(false);
            Destroy(stopWatch.gameObject);
            stopWatches.Remove(guid);
        }
    }
}
