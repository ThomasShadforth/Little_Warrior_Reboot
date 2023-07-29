using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    public enum Boss_Phases
    {
        waitingToStart,
        phase_1,
        phase_2,
        phase_3
    }

    [SerializeField] ColliderTrigger _trigger;
    [SerializeField] AIThinker _bossAI;

    [SerializeField] AIState _phase1State;
    [SerializeField] AIState _phase2State;
    [SerializeField] AIState _phase3State;

    Boss_Phases _phase;

    bool _bossStarted;

    private void Awake()
    {
        _phase = Boss_Phases.waitingToStart;
    }

    // Start is called before the first frame update
    void Start()
    {
        _trigger.OnPlayerEnterTrigger += ColliderTrigger_OnPlayerEnterTrigger;
        _bossAI.GetComponent<AIHealth>().GetHealthSystem().OnHealthChanged += _Boss_OnDamaged;
    }

    void ColliderTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        if (_bossStarted) return;
        _StartBossBattle();
        _bossStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _StartBossBattle()
    {
        _StartNextPhase();
    }

    void _StartNextPhase()
    {
        switch (_phase)
        {
            case Boss_Phases.waitingToStart:
                _phase = Boss_Phases.phase_1;
                _bossAI.TransitionToState(_phase1State);
                break;
            case Boss_Phases.phase_1:
                _phase = Boss_Phases.phase_2;
                _bossAI.TransitionToState(_phase2State);
                break;
            case Boss_Phases.phase_2:
                _phase = Boss_Phases.phase_3;
                _bossAI.TransitionToState(_phase3State);
                break;
        }
    }

    void _Boss_OnDamaged(object sender, System.EventArgs e)
    {
        switch (_phase)
        {
            default:
                break;
            case Boss_Phases.phase_1:
                break;
            case Boss_Phases.phase_2:
                break;
        }
    }
}
