using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageInterface
{
    public void DetectHit(int damageTaken, Transform hitOrigin = null);
}
