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
        //Debug.Log(_isKnocked);
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
                
                if (gameObject.activeInHierarchy && statusState)
                {
                    _isKnocked = statusState;
                    StartCoroutine(KnockbackTimerCo());
                } else if(_isKnocked && !statusState)
                {
                    GetComponent<Animator>().Play("Idle");
                    _isKnocked = statusState;
                }
                break;
            case StatusEnum.Dazed:
                break;
        }
    }
    
    IEnumerator KnockbackTimerCo()
    {
        float timePercentage = 0;

        while(timePercentage <= 1)
        {
            //Debug.Log("Knockback time for " + gameObject.name + ": " + timePercentage);
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
