using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerInputJump : MonoBehaviour
{
    [SerializeField] PlayerController _pc;

    private void Start()
    {
        //_pc = GetComponent<PlayerController>();
        Debug.Log(_pc);
        _pc.InputActions.Player.Jump.started += OnJump;
    }

    void OnJump(InputAction.CallbackContext context)
    {
            Debug.Log("Jump pressed");
        if (_pc.CharController.isGrounded)
        {
        }
    }
}
