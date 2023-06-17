using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using TMPro;

public class ControlUIHandler : MonoBehaviour
{
    [SerializeField] string _originalFieldText;
    [SerializeField] Image _iconImage;
    [SerializeField] ControlUIData _UIData;

    [SerializeField] TextMeshProUGUI _controlUIText;

    PlayerInput _playerInputHandler;

    private void OnEnable()
    {
        InputUser.onChange += _OnInputDeviceChange;
    }

    private void OnDisable()
    {
        InputUser.onChange -= _OnInputDeviceChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerInputHandler = GetComponent<PlayerInput>();
        _UpdateButtonImage(_playerInputHandler.currentControlScheme);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void _OnInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if(change == InputUserChange.ControlSchemeChanged)
        {
            //Debug.Log(user.controlScheme.Value.name);
            _UpdateButtonImage(user.controlScheme.Value.name);
        }
    }

    void _UpdateButtonImage(string controlSchemeName)
    {
        //_iconImage.sprite = _UIData.GetControlSchemeIcon(controlSchemeName);

        string outputText = "";
        //outputText = _originalFieldText.Replace("[Insert Icon Here]", $"<sprite name= \"{_UIData.GetIconAssetTag(controlSchemeName)}\">");

        outputText = _originalFieldText.Replace("[Insert Icon Here]", $"<sprite={_UIData.GetIconAssetIndex(controlSchemeName)}>");
        _controlUIText.text = outputText;
    }



    
}
