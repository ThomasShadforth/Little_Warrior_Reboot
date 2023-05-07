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

    private Button _saveSlotButton;

    private void Awake()
    {
        _saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if(data == null)
        {
            _noDataContent.SetActive(true);
            _hasDataContent.SetActive(false);
            //_clearButton.gameObject.SetActive(false);
        }
        else
        {
            _noDataContent.SetActive(false);
            _hasDataContent.SetActive(true);
            //_clearButton.gameObject.SetActive(true);
        }
    }

    public string GetProfileId()
    {
        return _profileId;
    }

    public void SetInteractable(bool interactable)
    {
        _saveSlotButton.interactable = interactable;
        //_clearButton.interactable = interactable;
    }
}
