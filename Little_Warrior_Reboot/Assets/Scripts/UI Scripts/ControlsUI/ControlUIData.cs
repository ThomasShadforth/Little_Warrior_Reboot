using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ControlUIData
{
    [SerializeField] TMP_SpriteAsset _iconsSpriteAsset;
    [SerializeField] Sprite _keyboardControlIcon;
    [SerializeField] Sprite _gamepadControlIcon;

    [SerializeField] string _keyboardIconName;
    [SerializeField] string _xboxGamepadIconName;


    public Sprite GetControlSchemeIcon(string controlSchemeType)
    {
        Sprite controlIcon = null;

        if(controlSchemeType == "XboxGamepad")
        {
            controlIcon = _gamepadControlIcon;
        }
        else
        {
            controlIcon = _keyboardControlIcon;
            //Return the thing for keyboard controls
        }
        return controlIcon;
    }

    public string GetIconAssetTag(string controlSchemeType)
    {
        string iconName = "";

        if(controlSchemeType == "XboxGamepad")
        {
            iconName = _xboxGamepadIconName;
        } else if(controlSchemeType == "Keyboard")
        {
            iconName = _keyboardIconName;
        }

        return iconName;
    }

    public int GetIconAssetIndex(string controlSchemeType)
    {
        Debug.Log(controlSchemeType);

        int keyboardIconIndex = _iconsSpriteAsset.GetSpriteIndexFromName(_keyboardIconName);
        int xboxGamepadIconIndex = _iconsSpriteAsset.GetSpriteIndexFromName(_xboxGamepadIconName);

        int returnedIndex = 0;

        if(controlSchemeType == "XboxGamepad")
        {
            returnedIndex = xboxGamepadIconIndex;
        }
        else
        {
            returnedIndex = keyboardIconIndex;
        }

        return returnedIndex;
    }
}
