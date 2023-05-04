using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation:")]
    [SerializeField] private SaveSlotMenu _saveSlotMenu;

    [Header("Menu Buttons:")]
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _loadGameButton;

    // Start is called before the first frame update
    void Start()
    {
        _DisableButtonsDependingData();
    }

    void _DisableButtonsDependingData()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            _continueGameButton.gameObject.SetActive(false);
            _loadGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        _saveSlotMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameClicked()
    {
        _saveSlotMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueGameClicked()
    {
        string sceneToLoad = DataPersistenceManager.instance.GetLastScene();
        //
        StartCoroutine(_LoadGameCo(sceneToLoad));
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        _DisableButtonsDependingData();
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void _DisableMenuButtons()
    {
        _newGameButton.interactable = false;
        _continueGameButton.interactable = false;
    }

    IEnumerator _LoadGameCo(string sceneToLoad)
    {
        if(UIScreenFade.instance != null)
        {
            UIScreenFade.instance.FadeToBlack();
        }

        DataPersistenceManager.instance.SaveGame();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
