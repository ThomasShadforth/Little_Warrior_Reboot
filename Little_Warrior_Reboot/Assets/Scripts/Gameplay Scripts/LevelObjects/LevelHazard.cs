using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHazard : MonoBehaviour
{
    [SerializeField] Vector2 _hazardKnockForce;
    [SerializeField] int _hazardDamage;
    [SerializeField] float _hazardKnockDuration;
    // Start is called before the first frame update
    

    public int GetHazardDamage()
    {
        return _hazardDamage;
    }

    public float GetHazardKnockDur()
    {
        return _hazardKnockDuration;
    }

    public Vector2 GetHazardKnockForce()
    {
        return _hazardKnockForce;
    }

}
