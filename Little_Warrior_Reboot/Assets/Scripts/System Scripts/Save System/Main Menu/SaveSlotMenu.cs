using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveSlotMenu : MonoBehaviour
{
    [Header("Menu Navigation:")]
    [SerializeField] private MainMenu _mainMenu;

    private SaveSlot[] _saveSlots;

    private bool _isLoading = false;

    [Header("Slot Button Parent:")]
    [SerializeField] private GameObject _slotButtonParent;

    [Header("Menu Buttons:")]
    [SerializeField] private Button _backButton;

    [Header("Confirmation Pop-up")]
    [SerializeField] private ConfirmationPopup _confirmationPopup;

    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        _saveSlots = _slotButtonParent.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        if (_isLoading)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileID(saveSlot.GetProfileId());
            _SaveGameAndLoadScene(saveSlot.lastSavedScene);
        } else if (saveSlot.hasData)
        {
            _confirmationPopup.ActivateMenu("Starting a new game will overwrite the currently saved data. Are you sure?",
                () =>
                {
                    DataPersistenceManager.instance.ChangeSelectedProfileID(saveSlot.GetProfileId());
                    DataPersistenceManager.instance.NewGame();
                    _SaveGameAndLoadScene();
                },
                () =>{
                    this.ActivateMenu(_isLoading);
                });
        }
        else
        {
            DataPersistenceManager.instance.ChangeSelectedProfileID(saveSlot.GetProfileId());
            DataPersistenceManager.instance.NewGame();
            _SaveGameAndLoadScene();
        }
    }

    public void OnClearClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();
        _confirmationPopup.ActivateMenu(
            "Are you sure you want to delete this save data?",
            () =>
            {
                DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
                ActivateMenu(_isLoading);
            },
            () =>
            {
                ActivateMenu(_isLoading);
            });
    }

    public void OnBackClicked()
    {
        //Return to the main menu
        StartCoroutine(BackClickedCo());
    }

    public void ActivateMenu(bool isLoadingTheGame)
    {
        this.gameObject.SetActive(true);

        this._isLoading = isLoadingTheGame;

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        _backButton.interactable = true;

        foreach(SaveSlot slot in _saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(slot.GetProfileId(), out profileData);

            slot.SetData(profileData);

            if(profileData == null && isLoadingTheGame)
            {
                slot.SetInteractable(false);
            }
            else
            {
                slot.SetInteractable(true);
            }

        }

        //Play animations where required
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    void _SaveGameAndLoadScene(string sceneToLoad = "")
    {
        DataPersistenceManager.instance.SaveGame();

        StartCoroutine(LoadSceneCo(sceneToLoad));
    }

    public void DisableMenuButtons()
    {
        foreach(SaveSlot slot in _saveSlots)
        {
            slot.SetInteractable(false);
        }

        _backButton.interactable = false;
    }

    IEnumerator LoadSceneCo(string sceneToLoad = "")
    {
        if (UIScreenFade.instance != null)
        {
            UIScreenFade.instance.FadeToBlack();
        }

        yield return new WaitForSeconds(1f);

        //Load the scene, determine how said scene is loaded;
        //For now, load the sample scene

        SceneManager.LoadSceneAsync("SampleScene");
    }

    IEnumerator BackClickedCo()
    {
        yield return new WaitForSeconds(1f);
        _mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }
}
