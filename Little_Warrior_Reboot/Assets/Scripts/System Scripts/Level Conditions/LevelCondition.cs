using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelCondition : MonoBehaviour
{
    public abstract void ActivateCondition();

    public abstract bool CheckCondition();

    public abstract void RespondToCondition();
}
