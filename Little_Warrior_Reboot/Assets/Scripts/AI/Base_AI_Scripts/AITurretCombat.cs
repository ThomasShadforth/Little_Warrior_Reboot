using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurretCombat : MonoBehaviour
{
    [SerializeField] float _attackWaitTime;
    [SerializeField] LayerMask _ignoreLayer;
    [SerializeField] LineRenderer _lineRenderer;

    LineOfSight2D _enemyLos;

    Vector3 _lastPlayerLocation;

    bool _isAttacking;
    float _attackWaitTimer;


    // Start is called before the first frame update
    void Start()
    {
        _enemyLos = GetComponent<LineOfSight2D>();
        SetAttackWaitTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAttackWaitTimer()
    {
        _isAttacking = false;
        _attackWaitTimer = _attackWaitTime;
    }

    public void TickAttackWaitTimer()
    {
        _attackWaitTimer -= GamePause.deltaTime;
    }

    public void AttackTarget()
    {
        _isAttacking = true;

        SetLastPlayerLocation();

        if(AudioManager.instance != null)
        {
            //Play the charge sfx here
        }
    }

    public void SetLaserLine()
    {
        if (_lineRenderer != null)
        {
            _lineRenderer.SetPositions(new Vector3[] { transform.position, _enemyLos.GetLastPlayerPosition() });
            _lineRenderer.startColor = Color.Lerp(Color.green, Color.red, _attackWaitTimer / _attackWaitTime);
            _lineRenderer.endColor = Color.Lerp(Color.green, Color.red, _attackWaitTimer / _attackWaitTime);
        }
    }

    public void FireTurretLaser()
    {
        Vector3 directionToTarget = (_lastPlayerLocation - transform.position).normalized;

        float distanceToLastTarget = Vector3.Distance(transform.position, _lastPlayerLocation);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionToTarget, distanceToLastTarget, ~_ignoreLayer);

        if (hits.Length != 0)
        {
            Debug.Log(hits[0].collider.gameObject.name);

            foreach (RaycastHit2D hit in hits)
            {
                Debug.DrawRay(hit.point, hit.normal * 40, Color.red);
                IDamageInterface damagedObject = hit.collider.gameObject.GetComponent<IDamageInterface>();
                if (damagedObject != null)
                {
                    damagedObject.DetectHit(10, default, 0);
                }

            }

            if (ExplosionObjectPool.instance != null)
            {
                GameObject explosionObject = ExplosionObjectPool.instance.GetFromPool();
                explosionObject.transform.position = hits[0].point;
            }

        }
    }

    public void SetLastPlayerLocation()
    {
        _lastPlayerLocation = GetComponent<AITurretData>().GetTargetTransform().position;
    }

    public void SetLaserLineStatus(bool setToActive = false)
    {
        _lineRenderer.enabled = setToActive;
    }

    public bool GetIsAttacking()
    {
        return _isAttacking;
    }

    public bool GetLessThanMaxAttackWaitTime()
    {
        return _attackWaitTimer < _attackWaitTime;
    }

    public bool GetAttackWaitTimeFinished()
    {
        return _attackWaitTimer <= 0;
    }
}
