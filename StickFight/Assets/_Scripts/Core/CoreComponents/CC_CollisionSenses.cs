using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CC_CollisionSenses : CoreComponent
{
    public Transform GroundCheck { get => _groundCheck; set => _groundCheck = value; }
    public Transform WallCheck { get => _wallCheck; set => _wallCheck = value; }
    public Transform LedgeCheck { get => _ledgeCheck; set => _ledgeCheck = value; }
    public Transform CeilingCheck { get => _ceilingCheck; set => _ceilingCheck = value; }
    public float GroundCheckRadius { get => _groundCheckRadius; set => _groundCheckRadius = value; }
    public float WallCheckDistance { get => _wallCheckDistance; set => _wallCheckDistance = value; }
    public float CeilingCheckRadius { get => _ceilingCheckRadius; set => _ceilingCheckRadius = value; }
    public LayerMask WhatIsGround { get => _whatIsGround; set => _whatIsGround = value; }

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _ledgeCheck;
    [SerializeField] private Transform _ceilingCheck;

    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _ceilingCheckRadius;

    [SerializeField] private LayerMask _whatIsGround;

    public bool Ground
    {
        get => Physics2D.OverlapCircle(_groundCheck.position, GroundCheckRadius, WhatIsGround);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(_wallCheck.position, Vector2.right * _core.Movement.FacingDirection, WallCheckDistance, WhatIsGround);
    }

    public bool WallBack
    {
        get => Physics2D.Raycast(_wallCheck.position, Vector2.right * -_core.Movement.FacingDirection, WallCheckDistance, WhatIsGround);
    }

    public bool Ledge
    {
        get => Physics2D.Raycast(_ledgeCheck.position, Vector2.right * _core.Movement.FacingDirection, WallCheckDistance, WhatIsGround);
    }

    public bool Ceiling
    {
        get => Physics2D.Raycast(_ceilingCheck.position, Vector2.up, CeilingCheckRadius, WhatIsGround);
    }
}