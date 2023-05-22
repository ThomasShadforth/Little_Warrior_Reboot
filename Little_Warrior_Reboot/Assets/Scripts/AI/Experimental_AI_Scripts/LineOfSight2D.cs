using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight2D : MonoBehaviour
{
    public float losRadius;
    [Range(0, 360)]
    public float losAngle;

    [SerializeField] GameObject _player;
    [SerializeField] Vector3 _offset;

    public LayerMask playerLayer;
    public LayerMask obstructionConfig;

    bool _canSeePlayer;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerMovement>().gameObject;
        StartCoroutine(_FOVCo());
    }

    IEnumerator _FOVCo()
    {
        float delay = .2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            _FOVCheck();
        }
    }

    void _FOVCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, losRadius, playerLayer);

        if(rangeChecks.Length != 0)
        {
            Transform targetTransform = rangeChecks[0].transform;

            Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;
            
            if(Vector3.Angle(transform.right, directionToTarget) < losAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

                if(!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionConfig))
                {
                    //Debug.Log("CAN SEE PLAYER");
                    _canSeePlayer = true;
                }
                else
                {
                    Debug.Log("CANT SEE PLAYER");
                    _canSeePlayer = false;
                }
            }
            else
            {
                _canSeePlayer = false;
            }
        } else
        {
            if (_canSeePlayer)
            {
                _canSeePlayer = false;
            }
        }
    }

    public bool GetCanSeePlayer()
    {
        return _canSeePlayer;
    }

    public GameObject GetPlayer()
    {
        return _player;
    }
}
