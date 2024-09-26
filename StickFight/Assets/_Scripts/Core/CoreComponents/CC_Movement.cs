using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Move the box collider stuff to the collision senses script and setup  _playerData variables
public class CC_Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }
    public BoxCollider2D Col { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }

    private Vector2 _workspace;

    public int FacingDirection { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        RB = GetComponentInParent<Rigidbody2D>();
        Col = GetComponentInParent<BoxCollider2D>();

        FacingDirection = 1;
    }

    public void LogicUpdate()
    {
        CurrentVelocity = RB.velocity;
    }

    public void ResetGravityScale()
    {
        //RB.gravityScale = _playerData.DefaultGravityScale;
        RB.gravityScale = 1.0f;
    }

    public void SetGravityScale(float newGravityScale)
    {
        RB.gravityScale = newGravityScale;
    }

    public void SetGravityScaleZero()
    {
        RB.gravityScale = 0;
    }

    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }
    public void SetVelocity(float vel, Vector2 angle, int direction)
    {
        angle.Normalize();
        _workspace.Set(angle.x * vel * direction, angle.y * vel);
        RB.velocity = _workspace;
        CurrentVelocity = _workspace;
    }

    public void SetVelocityX(float vel)
    {
        _workspace.Set(vel, CurrentVelocity.y);
        RB.velocity = _workspace;
        CurrentVelocity = _workspace;
    }

    public void SetVelocityY(float vel)
    {
        _workspace.Set(CurrentVelocity.x, vel);
        RB.velocity = _workspace;
        CurrentVelocity = _workspace;
    }

    public void SetLinearDrag(float val)
    {
        RB.drag = val;
    }

    public void ResetLinearDrag()
    {
        //RB.drag = _playerData.DefaultLinearDrag;
        RB.drag = 0;
    }

    public void SetBoxCollider(Vector2 offset, Vector2 size)
    {
        Col.size = size;
        Col.offset = offset;
    }

    public void Flip()
    {
        FacingDirection *= -1;
        RB.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
}
