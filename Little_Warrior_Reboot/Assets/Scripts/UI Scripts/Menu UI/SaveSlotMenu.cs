using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotMenu : Menu
{
    [Header("Menu Navigation:")]
    [SerializeField] MainMenu _mainMenu;

    [SerializeField] GameObject _slotsParent;

    [Header("Menu Buttons:")]
    [SerializeField] private Button _backButton;

    private SaveSlot[] _saveSlots;

    private bool _isLoadingGame = false;

    private void Awake()
    {
        _saveSlots = _slotsParent.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        _DisableMenuButtons();

        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if (!_isLoadingGame)
        {
            DataPersistenceManager.instance.NewGame();
        }

        DataPersistenceManager.instance.SaveGame();

        SceneManager.LoadScene("SampleScene");
    }

    public void OnBackButtonPressed()
    {
        _mainMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);

        this._isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        GameObject firstSelected = _backButton.gameObject;
        foreach(SaveSlot saveSlot in _saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(_backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void _DisableMenuButtons()
    {
        foreach(SaveSlot saveSlot in _saveSlots)
        {
            saveSlot.SetInteractable(false);
        }

        _backButton.interactable = false;
    }
}
