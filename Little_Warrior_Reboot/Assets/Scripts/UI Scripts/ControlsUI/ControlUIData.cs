using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ControlUIData
{
    [SerializeField] TMP_SpriteAsset _iconsSpriteAsset;
    [SerializeField] string _keyboardIconName;
    [SerializeField] string _xboxGamepadIconName;

    public int GetIconAssetIndex(string controlSchemeType)
    {
        //Debug.Log(controlSchemeType);

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
