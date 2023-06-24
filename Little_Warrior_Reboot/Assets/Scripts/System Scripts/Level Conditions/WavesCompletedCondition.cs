using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WavesCompletedCondition : LevelCondition
{
    [SerializeField] DefeatedEnemyCondition _firstWave;
    List<DefeatedEnemyCondition> _enemyWaveConditions = new List<DefeatedEnemyCondition>();

    bool _wavesStarted;

    // Start is called before the first frame update
    void Start()
    {
        //Either get component or get component in children
        _enemyWaveConditions.Clear();
        _enemyWaveConditions = GetComponentsInChildren<DefeatedEnemyCondition>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool CheckCondition()
    {
        bool allWavesCompleted = true;

        for(int i = 0; i < _enemyWaveConditions.Count; i++)
        {
            if (!_enemyWaveConditions[i].CheckCondition())
            {
                allWavesCompleted = false;
                i = _enemyWaveConditions.Count;
            }
        }

        return allWavesCompleted;
    }

    public override void RespondToCondition()
    {
        
    }

    public override void ActivateCondition()
    {
        _SpawnFirstWave();
    }

    //Spawn first wave when entering the trigger area
    void _SpawnFirstWave()
    {
        if (!_wavesStarted)
        {
            if (_firstWave != null)
            {
                _firstWave.ActivateCondition();
                _wavesStarted = true;
            }
        }
    }

    
}
