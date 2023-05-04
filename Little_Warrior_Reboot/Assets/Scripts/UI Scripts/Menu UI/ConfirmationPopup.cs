using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConfirmationPopup : MonoBehaviour
{
    [Header("Components:")]
    [SerializeField] private TextMeshProUGUI _displayText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    public void ActivateMenu(string displayText, UnityAction confirmAction, UnityAction cancelAction)
    {
        this.gameObject.SetActive(true);
        this._displayText.text = displayText;

        _confirmButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();

        _confirmButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            confirmAction();
        });

        _cancelButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            cancelAction();
        });
    }

    void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
