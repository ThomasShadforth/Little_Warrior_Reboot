using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    bool _isKnocked;
    bool _isDazed;
    bool _disabledHeightMaintenance;

    bool _invincible;

    float _knockbackTime;

    public bool CheckForStatus()
    {
        return (_isKnocked == true || _isDazed == true);
    }

    public bool GetInvincibility()
    {
        return _invincible;
    }

    public bool GetDisabledHeightMaintenance()
    {
        return _disabledHeightMaintenance;
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

    public void SetDisabledHeightMaintenance()
    {
        _disabledHeightMaintenance = true;

        StartCoroutine(EnableHeightMaintenanceCo());
    }

    public void SetInvincibility()
    {
        _invincible = true;

        //To do: insert coroutine that resets invincibility after a few seconds
        StartCoroutine(DisableInvincibilityCo());
    }

    IEnumerator KnockbackTimerCo()
    {
        yield return new WaitForSeconds(_knockbackTime);
        SetStatus(StatusEnum.Knockback, false);
    }

    IEnumerator EnableHeightMaintenanceCo()
    {
        yield return new WaitForSeconds(.15f);

        _disabledHeightMaintenance = false;
    }

    IEnumerator DisableInvincibilityCo()
    {
        float timePercentage = 0;

        bool flickerToTransparent = true;

        while(timePercentage < 1)
        {
            timePercentage += GamePause.deltaTime / 2f;

            if (flickerToTransparent)
            {
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, Mathf.MoveTowards(GetComponent<SpriteRenderer>().color.a, 0, 10f * GamePause.deltaTime));

                if(GetComponent<SpriteRenderer>().color.a <= 0)
                {
                    flickerToTransparent = false;
                }
            }
            
            if(!flickerToTransparent)
            {
                //Debug.Log("");
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, Mathf.MoveTowards(GetComponent<SpriteRenderer>().color.a, 1, 10f * GamePause.deltaTime));

                if (GetComponent<SpriteRenderer>().color.a >= 1)
                {
                    flickerToTransparent = true;
                }
            }

            yield return null;
        }

        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1);
        _invincible = false;
    }
}
