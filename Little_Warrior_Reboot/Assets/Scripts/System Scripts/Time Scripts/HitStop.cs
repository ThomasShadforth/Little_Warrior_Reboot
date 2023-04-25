using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool _waiting;

    public void StopTime(float duration)
    {
        if (_waiting)
        {
            return;
        }

        Time.timeScale = 0f;
        StartCoroutine(_WaitCo(duration));
    }

    IEnumerator _WaitCo(float duration)
    {
        _waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        _waiting = false;
    }
}
