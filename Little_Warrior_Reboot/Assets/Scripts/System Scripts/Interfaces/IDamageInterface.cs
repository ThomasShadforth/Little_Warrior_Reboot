using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageInterface
{
    public void DetectHit(int damageTaken, Vector2 knockbackApplied = default(Vector2), float knockbackDuration = 0);
}
