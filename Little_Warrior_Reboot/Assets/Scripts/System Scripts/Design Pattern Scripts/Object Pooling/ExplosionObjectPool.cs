using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObjectPool : MonoBehaviour
{
    [SerializeField] int _objectCount;
    [SerializeField] GameObject _explosionPrefab;

    Queue<GameObject> _availableObjects = new Queue<GameObject>();

    public static ExplosionObjectPool instance { get; private set; }

    private void Awake()
    {
        instance = this;
        _GrowPool();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _GrowPool()
    {
        for(int i = 0; i < _objectCount; i++)
        {
            var instanceToAdd = Instantiate(_explosionPrefab);
            instanceToAdd.transform.SetParent(this.transform);
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
