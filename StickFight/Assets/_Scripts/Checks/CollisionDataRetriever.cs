using System;
using UnityEngine;

namespace StickFight
{
    public class CollisionDataRetriever : MonoBehaviour
    {
        // global information
        public bool OnGround { get; private set; }
        public bool OnWall { get; private set; }
        public bool OnCeiling { get; private set; }
        public float Friction { get; private set; }
        public Vector2 ContactNormal { get; private set; }

        public bool[] OnGroundRays { get; private set; }
        public bool[] OnWallRays { get; private set; }
        public bool[] OnCeilingRays { get; private set; }

        private PhysicsMaterial2D _material;

        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private Transform[] _groundCheckRayPoints;
        [SerializeField] private float _groundCheckRayLength = 0.1f;
        [SerializeField] private bool _showGroundCheckRayGizmos = false;

        [SerializeField] private LayerMask _whatIsWall;
        [SerializeField] private Transform[] _wallCheckRayPoints;
        [SerializeField] private float _wallCheckRayLength = 0.1f;
        [SerializeField] private bool _showWallCheckRayGizmos = false;

        [SerializeField] private LayerMask _whatIsCeiling;
        [SerializeField] private Transform[] _ceilingCheckRayPoints;
        [SerializeField] private float _ceilingCheckRayLength = 0.1f;
        [SerializeField] private bool _showCeilingCheckRayGizmos = false;

        private Move _move;

        private void Awake()
        {
            OnGroundRays = new bool[_groundCheckRayPoints.Length];
            OnWallRays = new bool[_wallCheckRayPoints.Length];
            OnCeilingRays = new bool[_ceilingCheckRayPoints.Length];
            _move = GetComponent<Move>();
        }

        private void FixedUpdate()
        {
            //CheckForGroundContactRay();
            //CheckForWallContactRay();
            //CheckForCeilingContactRay();
        }

        private void CheckForCeilingContactRay()
        {
            RaycastHit2D hit;
            bool isHit = false;

            for (int i = 0; i < _ceilingCheckRayPoints.Length; i++)
            {
                hit = Physics2D.Raycast(_ceilingCheckRayPoints[i].position, Vector2.up, _ceilingCheckRayLength, _whatIsCeiling);
                if (hit)
                {
                    isHit = true;
                    OnCeilingRays[i] = true;
                }
                else
                    OnCeilingRays[i] = false;
            }

            OnCeiling = isHit;
        }

        private void CheckForWallContactRay()
        {
            RaycastHit2D hit;
            bool isHit = false;

            bool isFacingRight = _move.IsFacingRight;
            Vector3 dir = isFacingRight ? transform.right : -transform.right;

            for (int i = 0; i < _wallCheckRayPoints.Length; i++)
            {
                hit = Physics2D.Raycast(_wallCheckRayPoints[i].position, dir, _wallCheckRayLength, _whatIsWall);
                if (hit)
                {
                    isHit = true;
                    OnWallRays[i] = true;
                }
                else
                    OnWallRays[i] = false;
            }

            OnWall = isHit;
        }

        private void CheckForGroundContactRay()
        {
            RaycastHit2D hit;
            bool isHit = false;

            for (int i = 0; i < _groundCheckRayPoints.Length; i++)
            {
                hit = Physics2D.Raycast(_groundCheckRayPoints[i].position, Vector2.down, _groundCheckRayLength, _whatIsGround);
                if (hit)
                {
                    isHit = true;
                    OnGroundRays[i] = true;
                }
                else
                    OnGroundRays[i] = false;
            }

            OnGround = isHit;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Friction = 0;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            RetrieveFriction(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            RetrieveFriction(collision);
        }

        public void EvaluateCollision(Collision2D collision)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactNormal = collision.GetContact(i).normal;
            }
        }

        private void RetrieveFriction(Collision2D collision)
        {
            Friction = 0;

            if (collision.rigidbody.sharedMaterial != null)
            {

                _material = collision.rigidbody.sharedMaterial;
                Friction = _material.friction;
            }
        }

        private void OnDrawGizmos()
        {
            if (_showGroundCheckRayGizmos)
            {
                for (int i = 0; i < _groundCheckRayPoints.Length; i++)
                {
                    Gizmos.DrawLine(_groundCheckRayPoints[i].position, _groundCheckRayPoints[i].position + (Vector3.down * _groundCheckRayLength));
                }
            }

            if (_showWallCheckRayGizmos)
            {
                bool isFacingRight = _move.IsFacingRight;
                Vector3 dir = isFacingRight ? transform.right : -transform.right;

                for (int i = 0; i < _wallCheckRayPoints.Length; i++)
                {
                    Gizmos.DrawLine(_wallCheckRayPoints[i].position, _wallCheckRayPoints[i].position + (dir * _wallCheckRayLength));
                }
            }

            if (_showCeilingCheckRayGizmos)
            {
                for (int i = 0; i < _ceilingCheckRayPoints.Length; i++)
                {
                    Gizmos.DrawLine(_ceilingCheckRayPoints[i].position, _ceilingCheckRayPoints[i].position + (Vector3.up * _ceilingCheckRayLength));
                }
            }
        }
    }
}
