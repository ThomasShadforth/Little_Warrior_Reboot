using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatusDebugUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _groundedText;
    [SerializeField] TextMeshProUGUI _jumpingText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (FindObjectOfType<PlayerMovement>())
        {
            _groundedText.text = "Grounded: " + FindObjectOfType<PlayerMovement>().GetGroundedState();
        }

        if (FindObjectOfType<PlayerJump>())
        {
            _jumpingText.text = "Jumping: " + FindObjectOfType<PlayerJump>().GetIsJumping();
        }
    }
}
