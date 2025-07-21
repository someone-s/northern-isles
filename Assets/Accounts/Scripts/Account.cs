using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class Account : MonoBehaviour
{
    public AccountEffect Effect { get; private set; }

    [Serializable]
    public class Ledger
    {
        [SerializeField] private SerializedDictionary<string, List<Entry>> entries;
        public ReadOnlyDictionary<string, List<Entry>> Entries;

        public Ledger()
        {
            entries = new();
            Entries = new(entries);
        }

        public void Push(string group, Entry entry)
        {
            List<Entry> subset;
            if (!entries.TryGetValue(group, out subset))
            {
                subset = new();
                entries.Add(group, subset);
            }

            subset.Add(entry);
        }
    }

    [Serializable]
    public struct Entry
    {
        public object detail;
        public float amount;
    }
    [SerializeField] private List<Ledger> ledgers;
    public ReadOnlyCollection<Ledger> Ledgers;

    private void Awake()
    {
        Effect = GetComponentInChildren<AccountEffect>();

        ledgers = new();
        Ledgers = new(ledgers);

        ledgers.Add(new Ledger());
    }

    public void AddTransaction(string group, object detail, float amount)
    {
        Ledgers.Last().Push(group, new Entry { detail = detail, amount = amount });

        if (amount > 0f)
            Effect.OnPositiveChange();
        else if (amount < 0f)
            Effect.OnNegativeChange();
    }

}
