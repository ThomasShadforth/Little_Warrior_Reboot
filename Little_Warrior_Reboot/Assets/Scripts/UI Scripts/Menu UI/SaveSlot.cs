using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string _profileId = "";

    [Header("Content")]
    [SerializeField] GameObject _noDataContent;
    [SerializeField] GameObject _hasDataContent;

    [Header("Clear Button")]
    [SerializeField] private Button _clearButton;

    bool _hasData;

    [SerializeField] string _lastSavedScene;
    private Button _saveSlotButton;

    private void Awake()
    {
        _saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if(data == null)
        {
            _hasData = false;
            _noDataContent.SetActive(true);
            _hasDataContent.SetActive(false);
            _clearButton.gameObject.SetActive(false);
        }
        else
        {
            _hasData = true;

            _noDataContent.SetActive(false);
            _hasDataContent.SetActive(true);
            _lastSavedScene = data.lastSavedScene;
            _clearButton.gameObject.SetActive(true);
        }
    }

    public string GetLastSavedScene()
    {
        return _lastSavedScene;
    }

    public string GetProfileId()
    {
        return _profileId;
    }

    public bool GetHasData()
    {
        return _hasData;
    }

    public void SetInteractable(bool interactable)
    {
        _saveSlotButton.interactable = interactable;
        //_clearButton.interactable = interactable;
    }
}
