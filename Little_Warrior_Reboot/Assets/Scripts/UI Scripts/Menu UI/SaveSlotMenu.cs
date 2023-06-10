using System;
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

    [Header("Confirmation Pop-up")]
    [SerializeField] private ConfirmationPopupMenu _confirmationPopupMenu;

    private SaveSlot[] _saveSlots;

    private bool _isLoadingGame = false;

    private void Awake()
    {
        _saveSlots = _slotsParent.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        _DisableMenuButtons();

        //To do: Adjust save system with improvements/fixes (Outside of finding objects in scenes)

        if (_isLoadingGame)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            SaveGameAndLoadScene(saveSlot.GetLastSavedScene());
        } else if (saveSlot.GetHasData())
        {
            _confirmationPopupMenu.ActivateMenu("Starting a new game will overwrite the currently saved data. Are you sure?",
                () =>
                {
                    DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                    DataPersistenceManager.instance.NewGame();
                    SaveGameAndLoadScene();
                },
                () =>
                {
                    this.ActivateMenu(_isLoadingGame);
                }
                );
        }
        else
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            DataPersistenceManager.instance.NewGame();
            SaveGameAndLoadScene();
        }
    }

    private void SaveGameAndLoadScene(string sceneToLoad = "")
    {
        DataPersistenceManager.instance.SaveGame();
        StartCoroutine(LoadSceneCo());
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

    public void OnClearButtonClicked(SaveSlot saveSlot)
    {
        _DisableMenuButtons();
        _confirmationPopupMenu.ActivateMenu(
            "Are you sure you want to delete this save data?",
            () =>
            {
                DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
                ActivateMenu(_isLoadingGame);
            },
            () =>
            {
                _backButton.interactable = true;
                ActivateMenu(_isLoadingGame);
            }
            );
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

    IEnumerator LoadSceneCo(string sceneToLoad = "")
    {
        if(UIScreenFade.instance != null)
        {
            UIScreenFade.instance.FadeToBlack();
        }

        yield return new WaitForSeconds(1.5f);

        if(sceneToLoad == "")
        {
            //For now, load the first level.
            //Will need to work out a system for loading in/out the sandbox space
            SceneManager.LoadSceneAsync("Level_1_Draft");
        }
        else
        {
            //GameManager.instance.
            SceneManager.LoadSceneAsync(sceneToLoad);
        }
    }
}
