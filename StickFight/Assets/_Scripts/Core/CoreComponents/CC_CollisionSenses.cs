using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CC_CollisionSenses : CoreComponent
{
    public Transform GroundCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(_groundCheck, _core.transform.parent.name);
        set => _groundCheck = value;
    }
    public Transform WallCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(_wallCheck, _core.transform.parent.name);
        set => _wallCheck = value;
    }
    public Transform LedgeCheckHorizontal
    {
        get => GenericNotImplementedError<Transform>.TryGet(_ledgeCheckHorizontal, _core.transform.parent.name);
        set => _ledgeCheckHorizontal = value;
    }
    public Transform LedgeCheckVertical
    {
        get => GenericNotImplementedError<Transform>.TryGet(_ledgeCheckVertical, _core.transform.parent.name);
        set => _ledgeCheckVertical = value;
    }
    public Transform CeilingCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(_ceilingCheck, _core.transform.parent.name);
        set => _ceilingCheck = value;
    }

    public float GroundCheckRadius { get => _groundCheckRadius; set => _groundCheckRadius = value; }
    public float WallCheckDistance { get => _wallCheckDistance; set => _wallCheckDistance = value; }
    public float CeilingCheckRadius { get => _ceilingCheckRadius; set => _ceilingCheckRadius = value; }
    public LayerMask WhatIsGround { get => _whatIsGround; set => _whatIsGround = value; }

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _ledgeCheckHorizontal;
    [SerializeField] private Transform _ledgeCheckVertical;
    [SerializeField] private Transform _ceilingCheck;

    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _ceilingCheckRadius;

    [SerializeField] private LayerMask _whatIsGround;

    public bool Ground
    {
        get => Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, WhatIsGround);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * _core.Movement.FacingDirection, WallCheckDistance, WhatIsGround);
    }

    public bool WallBack
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * -_core.Movement.FacingDirection, WallCheckDistance, WhatIsGround);
    }

    // LedgeHorizontal is the check for the player to check if there is a ledge in front of them to climb on
    public bool LedgeHorizontal
    {
        get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * _core.Movement.FacingDirection, WallCheckDistance, WhatIsGround);
    }

    // LedgeVertical is the check for the enemy to check if there is a ledge below them so they don't walk off ledges
    public bool LedgeVertical
    {
        get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, WallCheckDistance, WhatIsGround);
    }

    public bool Ceiling
    {
        get => Physics2D.Raycast(CeilingCheck.position, Vector2.up, CeilingCheckRadius, WhatIsGround);
    }
}