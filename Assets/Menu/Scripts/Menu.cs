using System;
using System.Collections;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }
    private Menu()
    {
        Instance = this;
    }


    public bool allowManualSave = true;
    private bool firstGameLoaded = false;

    [SerializeField] private string startSaveName;
    [SerializeField] private string menuSaveName;
    [SerializeField] private CanvasGroup mainCanvasGroup;

    [SerializeField] private GameObject enterPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject loadPanel;
    [SerializeField] private Transform loadContent;
    [SerializeField] private GameObject loadPrefab;
    [SerializeField] private Button returnButton;

    private void Start()
    {
        StateTrack.Instance.LoadResourceState(menuSaveName);
        mainCanvasGroup.alpha = 0;
        mainCanvasGroup.interactable = false;

        startButton.onClick.AddListener(OnStartPressed);
        loadButton.onClick.AddListener(OnLoadPressed);
        resumeButton.onClick.AddListener(OnResumePressed);
        saveButton.onClick.AddListener(OnSavePressed);
        exitButton.onClick.AddListener(OnExitPressed);
        returnButton.onClick.AddListener(OnReturnPressed);

        SetupMenu();
    }


    public void ShowMenu()
    {
        SetupMenu();
        firstGameLoaded = true;
    }

    private void SetupMenu()
    {
        enterPanel.SetActive(true);
        loadPanel.SetActive(false);

        gameObject.SetActive(true);
        mainCanvasGroup.alpha = 0;
        mainCanvasGroup.interactable = false;

        resumeButton.interactable = (StateTrack.Instance.AvailableSaves.Count() >= 1) || firstGameLoaded;
        saveButton.interactable = allowManualSave && firstGameLoaded;
    
    }

    private void OnStartPressed()
    {
        gameObject.SetActive(false);

        StateTrack.Instance.LoadResourceState(startSaveName);
        mainCanvasGroup.alpha = 1;
        mainCanvasGroup.interactable = true;
    }

    private void OnLoadPressed()
    {
        for (int i = loadContent.childCount - 1; i >= 0; i--)
            Destroy(loadContent.GetChild(i).gameObject);
        loadContent.DetachChildren();

        foreach ((string name, DateTime lastWrite) save in StateTrack.Instance.AvailableSaves.OrderByDescending(pair => pair.lastWrite))
        {
            string capturedName = save.name;
            GameObject saveEntryObject = Instantiate(loadPrefab, loadContent);
            saveEntryObject.GetComponentInChildren<TMP_Text>().text = save.lastWrite.ToString();
            saveEntryObject.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                gameObject.SetActive(false);

                // reset value 
                allowManualSave = true;

                StateTrack.Instance.LoadState(capturedName);
                mainCanvasGroup.alpha = 1;
                mainCanvasGroup.interactable = true;
            });
        }

        loadContent.GetComponent<OnDemandVerticalLayout>().Refresh();

        enterPanel.SetActive(false);
        loadPanel.SetActive(true);
    }

    private void OnReturnPressed()
    {
        enterPanel.SetActive(true);
        loadPanel.SetActive(false);
    }

    private void OnResumePressed()
    {
        if (!firstGameLoaded)
        {
            (string name, DateTime lastWrite) save = StateTrack.Instance.AvailableSaves.OrderBy(pair => pair.lastWrite).Last();
            StateTrack.Instance.LoadState(save.name);
        }


        gameObject.SetActive(false);

        mainCanvasGroup.alpha = 1;
        mainCanvasGroup.interactable = true;
    }

    private void OnSavePressed()
    {
        StateTrack.Instance.SaveQuickState();
    }

    private void OnExitPressed()
    {
        Application.Quit();
    }
}
