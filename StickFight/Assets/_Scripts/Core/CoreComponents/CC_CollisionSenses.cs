using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CC_CollisionSenses : CoreComponent
{
    public Transform GroundCheck
    {
        get
        {
            if (_groundCheck) return _groundCheck;

            Debug.LogError("No GroundCheck Defined for " + transform.parent.name);
            return null;
        }
        set => _groundCheck = value;
    }
    public Transform WallCheck
    {
        get
        {
            if (_wallCheck) return _wallCheck;

            Debug.LogError("No WallCheck Defined for " + transform.parent.name);
            return null;
        }
        set => _wallCheck = value;
    }
    public Transform LedgeCheckHorizontal
    {
        get
        {
            if (_ledgeCheckHorizontal) return _ledgeCheckHorizontal;

            Debug.LogError("No LedgeCheckHorizontal Defined for " + transform.parent.name);
            return null;
        }
        set => _ledgeCheckHorizontal = value;
    }
    public Transform LedgeCheckVertical
    {
        get
        {
            if (_ledgeCheckVertical) return _ledgeCheckVertical;

            Debug.LogError("No LedgeCheckVertical Defined for " + transform.parent.name);
            return null;
        }
        set => _ledgeCheckVertical = value;
    }
    public Transform CeilingCheck
    {
        get
        {
            if (_ceilingCheck) return _ceilingCheck;

            Debug.LogError("No ceilingCheck Defined for " + transform.parent.name);
            return null;
        }
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