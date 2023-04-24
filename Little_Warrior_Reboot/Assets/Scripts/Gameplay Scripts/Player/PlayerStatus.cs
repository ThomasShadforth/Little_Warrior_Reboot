using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    bool _isKnocked;
    bool _isDazed;
    float _knockbackTime;

    public bool CheckForStatus()
    {
        return (_isKnocked == true || _isDazed == true);
    }

    public void SetKnockbackTime(float knockbackDuration)
    {
        _knockbackTime = knockbackDuration;
    }

    public void SetStatus(StatusEnum statusToSet, bool statusState)
    {
        switch (statusToSet)
        {
            case StatusEnum.Knockback:
                _isKnocked = statusState;
                StartCoroutine(KnockbackTimerCo());
                break;
            case StatusEnum.Dazed:
                break;
        }
    }

    IEnumerator KnockbackTimerCo()
    {
        yield return new WaitForSeconds(_knockbackTime);
        SetStatus(StatusEnum.Knockback, false);
    }
}
