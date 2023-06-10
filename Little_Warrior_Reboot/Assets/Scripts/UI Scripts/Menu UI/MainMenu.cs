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

    [Header("Additional Configuration")]
    [SerializeField] string _firstLevelString;

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
            SceneManager.LoadSceneAsync("Level_2_Draft");
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
        string sceneToLoad = DataPersistenceManager.instance.GetProfileLastSavedScene();
        //DataPersistenceManager.instance.LoadGame();
        //DataPersistenceManager.instance.SaveGame();

        //SceneManager.LoadSceneAsync("Level_2_Draft");

        StartCoroutine(_LoadGameCo(sceneToLoad));
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

    IEnumerator _LoadGameCo(string sceneToLoad)
    {
        if(UIScreenFade.instance != null)
        {
            UIScreenFade.instance.FadeToBlack();
        }

        DataPersistenceManager.instance.SaveGame();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
