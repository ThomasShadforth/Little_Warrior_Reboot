using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStatus : MonoBehaviour
{
    bool _isKnocked;
    bool _isDazed;
    bool _disabledHeightMaintenance;

    [SerializeField] float _knockTime;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public bool CheckForStatus()
    {
        //At the moment, use a general check for status conditions. Will replace with more specific ones later down the line
        return (_isKnocked == true || _isDazed == true);
    }

    public bool GetDisabledHeightMaintenance()
    {
        return _disabledHeightMaintenance;
    }

    public void SetKnockbackTime(float knockTime)
    {
        _knockTime = knockTime;
    }

    public void SetDisabledHeightMaintenance()
    {
        _disabledHeightMaintenance = true;

        StartCoroutine(EnableHeightMaintenanceCo());
    }

    public void SetStatus(StatusEnum statusToSet, bool statusState)
    {
        switch (statusToSet)
        {
            case StatusEnum.Knockback:
                _isKnocked = statusState;
                if (gameObject.activeInHierarchy)
                {
                    StartCoroutine(KnockbackTimerCo());
                }
                break;
            case StatusEnum.Dazed:
                break;
        }
    }
    
    IEnumerator KnockbackTimerCo()
    {
        float timePercentage = 0;

        while(timePercentage < 1)
        {
            timePercentage += GamePause.deltaTime / _knockTime;
            yield return null;
        }

        SetStatus(StatusEnum.Knockback, false);
    }

    IEnumerator EnableHeightMaintenanceCo()
    {
        yield return new WaitForSeconds(.3f);

        _disabledHeightMaintenance = false;
    }

}
