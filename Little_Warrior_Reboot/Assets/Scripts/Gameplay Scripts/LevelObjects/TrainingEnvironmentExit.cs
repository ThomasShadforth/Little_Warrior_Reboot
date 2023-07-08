using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TrainingEnvironmentExit : MonoBehaviour
{
    //Will load the first level in the event there is no previous level that the player loaded in from
    [SerializeField] string _firstLevelName;

    [SerializeField] ConfirmationPopupMenu _confirmationPopup;

    bool _playerInRange;

    PlayerActionMap _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerActionMap();
        _playerInput.Player.Interact.Enable();
        _playerInput.Player.Interact.started += _InteractWithExit;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _InteractWithExit(InputAction.CallbackContext context)
    {
        if(context.started && _playerInRange)
        {
            //Activate the popup menu

            GamePause.paused = true;

            if(_confirmationPopup != null)
            {
                _confirmationPopup.ActivateMenu("Are you sure you would like to leave the training environment?",
                    () =>
                    {
                        GamePause.paused = false;
                        //Load the last saved scene if not empty
                        if(LevelNameData.GetLastLevelName() != "")
                        {
                            Debug.Log("LEVEL NAME NOT EMPTY");
                            StartCoroutine(_LoadLevelCo(LevelNameData.GetLastLevelName()));
                        }
                        else
                        {
                            //If empty, load the first level
                            StartCoroutine(_LoadLevelCo(_firstLevelName));
                        }
                        
                    },
                    () =>
                    {
                        GamePause.paused = false;
                    });
            }

        }
    }

    IEnumerator _LoadLevelCo(string levelToLoad)
    {
        if (UIScreenFade.instance != null)
        {
            UIScreenFade.instance.FadeToBlack();
        }

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadSceneAsync(levelToLoad);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }

}
