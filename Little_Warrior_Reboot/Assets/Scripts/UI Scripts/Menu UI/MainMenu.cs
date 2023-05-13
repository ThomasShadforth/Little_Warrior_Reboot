using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotMenu _saveSlotMenu;

    [Header("Menu Buttons:")]
    [SerializeField] Button _newGameButton;
    [SerializeField] Button _continueButton;
    [SerializeField] Button _loadGameButton;

    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            _continueButton.interactable = false;
            _loadGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        if (DataPersistenceManager.instance.GetDisabledDataPersistence())
        {
            SceneManager.LoadSceneAsync("SampleScene");
        }
        else
        {
            _saveSlotMenu.ActivateMenu(false);
            this.DeactivateMenu();
        }
    }

    public void OnLoadGameClicked()
    {
        _saveSlotMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnQuitGameClicked()
    {
        Application.Quit();
    }

    public void OnContinueGameClicked()
    {
        _DisableMenuButtons();
        //DataPersistenceManager.instance.LoadGame();
        DataPersistenceManager.instance.SaveGame();

        SceneManager.LoadSceneAsync("SampleScene");
    }

    private void _DisableMenuButtons()
    {
        _newGameButton.interactable = false;
        _continueButton.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
