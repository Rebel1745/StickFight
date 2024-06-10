using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StickFight
{
    public class Gravity : MonoBehaviour
    {
        private Rigidbody2D _body;

        private float _defaultGravityScale;

        private bool _ignoreLowPriorityGravity = false;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _defaultGravityScale = _body.gravityScale;
        }

        public void SetGravity(float amount, bool forceChange, bool ignoreLowPriority, string source)
        {
            if (amount != _body.gravityScale)
            {
                if (!_ignoreLowPriorityGravity || forceChange)
                {
                    //print("SetGravity()::Source: " + source);
                    _body.gravityScale = amount;
                    _ignoreLowPriorityGravity = ignoreLowPriority;
                }
            }
        }

        public void ZeroGravity(bool forceChange, bool ignoreLowPriority, string source)
        {
            if (_body.gravityScale != 0f)
            {
                if (!_ignoreLowPriorityGravity || forceChange)
                {
                    //print("ZeroGravity()::Source: " + source);
                    _body.gravityScale = 0f;
                    _body.velocity = new Vector2(_body.velocity.x, 0f);
                    _ignoreLowPriorityGravity = ignoreLowPriority;
                }
            }
        }

        public void ResetToDefaultGravity(bool forceChange, bool ignoreLowPriority, string source)
        {
            if (_body.gravityScale != _defaultGravityScale)
            {
                if (!_ignoreLowPriorityGravity || forceChange)
                {
                    //print("ResetToDefaultGravity()::Source: " + source);
                    _body.gravityScale = _defaultGravityScale;
                    _ignoreLowPriorityGravity = ignoreLowPriority;
                }
            }
        }
    }
}
