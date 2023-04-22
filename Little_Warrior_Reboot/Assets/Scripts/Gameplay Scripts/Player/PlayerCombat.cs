using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    //For now, simply play the test animation for the sake of testing player/enemy interaction
    //Eventually this will be expanded.

    [SerializeField] Transform _hitDetectPoint;
    [SerializeField] float _hitDetectRadius;
    IDamageInterface _playerDamageInterface;

    PlayerActionMap _playerInput;


    private void Awake()
    {
        _playerInput = new PlayerActionMap();
        _playerInput.Player.TestPunch.Enable();

        _playerInput.Player.TestPunch.started += _TestPunchInput;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerDamageInterface = GetComponent<IDamageInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackHitDetect()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(_hitDetectPoint.position, _hitDetectRadius);

        foreach(Collider2D hitObject in hitObjects)
        {
            IDamageInterface damageInterface = hitObject.GetComponent<IDamageInterface>();

            if(damageInterface != null && damageInterface != _playerDamageInterface)
            {
                damageInterface.DetectHit(20);
            }

        }
    }

    public void ResetAttackAnimation()
    {
        GetComponent<Animator>().Play("Idle");
    }

    void _TestPunchInput(InputAction.CallbackContext context)
    {
        //Debug.Log("PUNCHING");
        GetComponent<Animator>().Play("PlayerPunch1");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_hitDetectPoint.position, _hitDetectRadius);
    }
}
