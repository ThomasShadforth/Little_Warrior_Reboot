using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    PlayerActionMap _playerInput;

    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject[] _menuWindows;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = new PlayerActionMap();
        _playerInput.Player.PauseGame.Enable();
        _playerInput.Player.PauseGame.started += PauseGameInput;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenuWindow(int menuIndex)
    {
        _CloseWindows();
        _menuWindows[menuIndex].SetActive(true);
    }

    void _CloseWindows()
    {
        foreach(GameObject window in _menuWindows)
        {
            window.SetActive(false);
        }
    }

    public void UnpauseGame()
    {
        GamePause.paused = false;
        _CloseWindows();
        _playerInput.Player.Enable();
        _pauseMenu.SetActive(false);
    }

    //To do: Insert logic for returning to the main menu
    public void QuitToMenu()
    {

    }

    void PauseGameInput(InputAction.CallbackContext context)
    {
        if (!_pauseMenu.activeInHierarchy)
        {
            //Open the pause menu, pause the game
            GamePause.paused = true;
            _playerInput.Player.Disable();
            _pauseMenu.SetActive(true);
            //Enable the UI input;
            //Set the current selected button (After setting up UI links)
        }


    }
}
