using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController _cc;
    Animator _anim;
    PlayerInputActions _pa;

    Vector3 _moveInput;

    #region Player Variables
    [SerializeField] float _movementSpeed = 5f;

    #endregion

    #region Getters and Setters
    public PlayerInputActions InputActions { get { return _pa; } }
    public CharacterController CharController { get { return _cc; } }
    #endregion

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();

        _pa = new PlayerInputActions();
        _pa.Player.Movement.started += UpdateMovementInput;
        _pa.Player.Movement.performed += UpdateMovementInput;
        _pa.Player.Movement.canceled += UpdateMovementInput;
    }

    private void Update()
    {
        _cc.Move(_moveInput * Time.deltaTime * _movementSpeed);
    }

    void UpdateMovementInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>().normalized;
        _moveInput = new Vector3(input.x, 0f, 0f);
    }

    private void OnEnable()
    {
        _pa.Enable();
    }

    private void OnDisable()
    {
        _pa.Disable();
    }
}
