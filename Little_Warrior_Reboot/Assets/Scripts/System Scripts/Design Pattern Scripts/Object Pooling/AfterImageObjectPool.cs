using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageObjectPool : MonoBehaviour
{
    [SerializeField] int _afterImageCount;
    [SerializeField] GameObject _afterImagePrefab;

    Queue<GameObject> _availableObjects = new Queue<GameObject>();

    public static AfterImageObjectPool instance { get; private set; }

    private void Awake()
    {
        instance = this;
        _GrowPool();
    }

    void _GrowPool()
    {
        for (int i = 0; i < _afterImageCount; i++)
        {
            var instanceToAdd = Instantiate(_afterImagePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instanceToAdd)
    {
        instanceToAdd.SetActive(false);
        _availableObjects.Enqueue(instanceToAdd);
    }

    public GameObject GetFromPool()
    {
        if(_availableObjects.Count == 0)
        {
            _GrowPool();
        }

        var poolInstance = _availableObjects.Dequeue();
        poolInstance.SetActive(true);

        return poolInstance;
    }
}
