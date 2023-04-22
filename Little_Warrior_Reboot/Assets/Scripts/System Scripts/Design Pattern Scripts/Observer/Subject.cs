using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    List<IObserver> _observers = new List<IObserver>();

    protected void _NotifyObservers(int damageTaken)
    {
        foreach(IObserver observer in _observers)
        {
            observer.OnNotify(damageTaken);
        }
    }

    public void BeginNotify(int damageTaken)
    {
        _NotifyObservers(damageTaken);
    }

    public void GetObserverCount()
    {
        Debug.Log(_observers.Count);
    }

    public void AddObserver(IObserver observer)
    {
        if(observer == null)
        {
            Debug.Log("NOT FOUND");
            return;
        }

        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        if(observer == null || !_observers.Contains(observer))
        {
            Debug.Log("OBSERVER DOESNT EXIST");
            return;
        }

        _observers.Remove(observer);
    }
}
