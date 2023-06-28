using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedEnemyCondition : LevelCondition, IObserver
{
    [SerializeField] LevelCondition _nextWaveToSpawn;
    [SerializeField] GameObject[] _enemiesToSpawn;

    bool _waveSpawned;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject enemy in _enemiesToSpawn)
        {
            Subject enemySubject = enemy.GetComponent<Subject>();

            if(enemySubject != null)
            {
                enemySubject.AddObserver(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool CheckCondition()
    {
        bool allEnemiesDefeated = true;

        if (!_waveSpawned)
        {
            Debug.Log("WAVE HAS NOT BEEN SPAWNED");
            //return false
            return false;
        }
        else
        {
            for(int i = 0; i < _enemiesToSpawn.Length; i++)
            {
                if (_enemiesToSpawn[i].activeInHierarchy)
                {
                    Debug.Log("NOT ALL ENEMIES DEFEATED, " + _enemiesToSpawn[i].name + " IS STILL ALIVE");
                    allEnemiesDefeated = false;
                    i = _enemiesToSpawn.Length;
                }
            }
        }

        return allEnemiesDefeated;
    }

    public override void RespondToCondition()
    {
        
    }

    public override void ActivateCondition()
    {
        SpawnWave();
    }

    public void SpawnWave()
    {
        if (!_waveSpawned)
        {
            foreach(GameObject enemy in _enemiesToSpawn)
            {
                enemy.SetActive(true);
            }
        }

        _waveSpawned = true;
    }

    public void OnNotify(int damageTaken)
    {
        if (damageTaken > 0) return;

        StartCoroutine(DelayedConditionCheckCo());
    }

    IEnumerator DelayedConditionCheckCo()
    {
        yield return new WaitForSeconds(.2f);

        bool enemiesDefeated = CheckCondition();

        if (enemiesDefeated)
        {
            Debug.Log("ALL ENEMIES IN WAVE DEFEATED");
            //Spawn the next wave of enemies
            _nextWaveToSpawn.ActivateCondition();
        }
    }
}
