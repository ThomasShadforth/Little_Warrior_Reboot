using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.InputSystem;

public class CinemachineStateDrivenCam : MonoBehaviour
{
    public static CinemachineStateDrivenCam instance;

    CinemachineStateDrivenCamera _stateCam;

    [SerializeField] string _playerCamName;
    [SerializeField] List<string> _camNames = new List<string>();

    Animator _animator;

    PlayerActionMap _playerInput;

    private void Awake()
    {
        _stateCam = GetComponent<CinemachineStateDrivenCamera>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _ParentCinemachineCams();
        _ResetCamNamesList();
        _SetCamStates();
        ChangeState(_playerCamName);
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = new PlayerActionMap();
        _playerInput.Player.TestRestart.Enable();
        _playerInput.Player.TestRestart.started += _TestCamChange;

        instance = this;
    }

    public void ChangeState(string nameOfCam)
    {
        for(int i = 0; i < _camNames.Count; i++)
        {
            if(_camNames[i] == nameOfCam)
            {
                _animator.Play(_camNames[i]);
                i = _camNames.Count;
            }
        }
    }

    void _ParentCinemachineCams()
    {
        CinemachineVirtualCamera[] cineCams = FindObjectsOfType<CinemachineVirtualCamera>();

        foreach(CinemachineVirtualCamera cineCam in cineCams)
        {
            cineCam.transform.parent = this.transform;
        }
    }

    void _ResetCamNamesList()
    {
        _camNames.Clear();

        CinemachineVirtualCamera[] cineCams = GetComponentsInChildren<CinemachineVirtualCamera>();

        foreach(CinemachineVirtualCamera cineCam in cineCams)
        {
            _camNames.Add(cineCam.gameObject.name);
        }
        //Note: May need to get camera names from new child objects (Which are set on load)
    }

    void _SetCamStates()
    {
        _stateCam.m_Instructions = new CinemachineStateDrivenCamera.Instruction[_camNames.Count];

        for(int i = 0; i < _camNames.Count; i++)
        {
            _stateCam.m_Instructions[i].m_FullHash = Animator.StringToHash("Base Layer." + _camNames[i]);
            _stateCam.m_Instructions[i].m_VirtualCamera = _GetChildCamera(_camNames[i]);
        }
    }

    CinemachineVirtualCamera _GetChildCamera(string camName)
    {
        CinemachineVirtualCamera[] cineCams = GetComponentsInChildren<CinemachineVirtualCamera>();

        foreach(CinemachineVirtualCamera cineCam in cineCams)
        {
            if(cineCam.gameObject.name == camName)
            {
                return cineCam;
            }
        }

        return null;
    }

    void _TestCamChange(InputAction.CallbackContext context)
    {
        ChangeState("SecondCam");
    }
}
