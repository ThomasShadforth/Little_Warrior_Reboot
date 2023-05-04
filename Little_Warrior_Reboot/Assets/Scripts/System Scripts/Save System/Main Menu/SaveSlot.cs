using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile:")]
    [SerializeField] private string _profileId = "";

    public string lastSavedScene { get; private set; } = "";

    [Header("Content:")]
    [SerializeField] private GameObject _noDataContent;
    [SerializeField] private GameObject _hasDataContent;
    [SerializeField] private TextMeshProUGUI _dateTimeText;

    [Header("Clear Data Button")]
    [SerializeField] private Button clearDataButton;

    public bool hasData { get; private set; } = false;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if(data == null)
        {
            hasData = false;
            _noDataContent.SetActive(true);
            _hasDataContent.SetActive(false);
            clearDataButton.gameObject.SetActive(false);
        }
        else
        {
            hasData = true;
            _noDataContent.SetActive(false);
            _hasDataContent.SetActive(true);
            clearDataButton.gameObject.SetActive(true);

            lastSavedScene = data.lastSceneSaved;
            _dateTimeText.text = "LAST SAVED: " + DateTime.FromBinary(data.lastSaved);
        }
    }

    public string GetLastScene()
    {
        return this.lastSavedScene;
    }

    public string GetProfileId()
    {
        return this._profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        clearDataButton.interactable = interactable;
    }

}
