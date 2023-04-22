using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    //Decide what data needs to be passed via the observer pattern
    public void OnNotify(int damageTaken);
}
