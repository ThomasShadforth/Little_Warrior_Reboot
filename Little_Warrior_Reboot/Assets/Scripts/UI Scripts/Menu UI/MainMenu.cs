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
    [SerializeField] string _debugLevelString;
    [SerializeField] string _trainingAreaString;
    [SerializeField] string _firstLevelString;
    [SerializeField] ConfirmationPopupMenu _confirmationPopup;

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
            _CheckTrainingLevelDecision();
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

    void _CheckTrainingLevelDecision()
    {
        if(_confirmationPopup != null)
        {
            _confirmationPopup.ActivateMenu("Would you like to enter the training environment to learn your basic abilities?",
                () =>
                {
                    //The scene
                    StartCoroutine(_LoadGameCo(_trainingAreaString));
                },
                () =>
                {
                    StartCoroutine(_LoadGameCo(_firstLevelString));
                });
        }
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
