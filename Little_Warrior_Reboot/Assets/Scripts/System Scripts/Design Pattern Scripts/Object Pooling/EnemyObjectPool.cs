using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour
{
    Queue<GameObject> _availableObjects;

    public static EnemyObjectPool instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _availableObjects = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToPool(GameObject instanceToAdd)
    {
        instanceToAdd.SetActive(false);
        _availableObjects.Enqueue(instanceToAdd);
    }

    public GameObject GetFromPool()
    {
        if (_availableObjects.Count == 0) return null;

        var poolInstance = _availableObjects.Dequeue();
        poolInstance.SetActive(true);

        return poolInstance;
    }
}
