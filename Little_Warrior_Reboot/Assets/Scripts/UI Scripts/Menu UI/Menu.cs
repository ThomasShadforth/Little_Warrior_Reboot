using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [Header("First Selected Button")]
    [SerializeField] private Button _firstSelected;

    protected virtual void OnEnable() 
    {
        SetFirstSelected(_firstSelected);
    }

    public void SetFirstSelected(Button firstSelectedButton)
    {
        firstSelectedButton.Select();

    }
}
