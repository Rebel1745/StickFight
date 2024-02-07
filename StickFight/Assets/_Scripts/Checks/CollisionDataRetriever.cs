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

        [SerializeField] private bool _showGroundCheckGizmo = false;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private Vector2 _groundCheckSize;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private Transform[] _groundCheckRayPoints;
        [SerializeField] private float _groundCheckRayLength = 0.1f;
        [SerializeField] private bool _showGroundCheckRayGizmos = false;

        [SerializeField] private bool _showWallCheckGizmo = false;
        [SerializeField] private Transform _wallCheck;
        [SerializeField] private Vector2 _wallCheckSize;
        [SerializeField] private LayerMask _whatIsWall;
        [SerializeField] private Transform[] _wallCheckRayPoints;
        [SerializeField] private float _wallCheckRayLength = 0.1f;
        [SerializeField] private bool _showWallCheckRayGizmos = false;

        [SerializeField] private bool _showCeilingCheckGizmo = false;
        [SerializeField] private Transform _ceilingCheck;
        [SerializeField] private Vector2 _ceilingCheckSize;
        [SerializeField] private LayerMask _whatIsCeiling;
        [SerializeField] private Transform[] _ceilingCheckRayPoints;
        [SerializeField] private float _ceilingCheckRayLength = 0.1f;
        [SerializeField] private bool _showCeilingCheckRayGizmos = false;

        private void Awake()
        {
            OnGroundRays = new bool[_groundCheckRayPoints.Length];
            OnWallRays = new bool[_wallCheckRayPoints.Length];
            OnCeilingRays = new bool[_ceilingCheckRayPoints.Length];
        }

        private void FixedUpdate()
        {
            //CheckForGroundContact();
            CheckForGroundContactRay();
            //CheckForWallContact();
            CheckForWallContactRay();
            //CheckForCeilingContact();
            CheckForCeilingContactRay();
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

            for (int i = 0; i < _wallCheckRayPoints.Length; i++)
            {
                hit = Physics2D.Raycast(_wallCheckRayPoints[i].position, transform.right, _wallCheckRayLength, _whatIsWall);
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

        /*void CheckForGroundContact()
        {
            if (_groundCheck == null)
                return;

            OnGround = Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize, 0f, _whatIsGround);
        }

        void CheckForWallContact()
        {
            if (_wallCheck == null)
                return;

            OnWall = Physics2D.OverlapBox(_wallCheck.position, _wallCheckSize, 0f, _whatIsWall);
        }

        void CheckForCeilingContact()
        {
            if (_ceilingCheck == null)
                return;

            OnCeiling = Physics2D.OverlapBox(_ceilingCheck.position, _ceilingCheckSize, 0f, _whatIsCeiling);
        }*/

        private void OnCollisionExit2D(Collision2D collision)
        {
            //OnGround = false;
            Friction = 0;
            //OnWall = false;
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
                /*if (ContactNormal.y >= 0.9f && collision.gameObject.layer == _whatIsGround)
                    OnGround = true;
                else OnGround = false;

                if (Mathf.Abs(ContactNormal.x) >= 0.9f && collision.gameObject.layer == _whatIsWall)
                    OnWall = true;
                else OnWall = false;*/
            }
        }

        private void RetrieveFriction(Collision2D collision)
        {
            Friction = 0;

            if(collision.rigidbody.sharedMaterial != null)
            {

                _material = collision.rigidbody.sharedMaterial;
                Friction = _material.friction;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if(_showGroundCheckGizmo)
                Gizmos.DrawWireCube(_groundCheck.position, new Vector3(_groundCheckSize.x, _groundCheckSize.y, 1f));
            if(_showWallCheckGizmo)
                Gizmos.DrawWireCube(_wallCheck.position, new Vector3(_wallCheckSize.x, _wallCheckSize.y, 1f));
            if(_showCeilingCheckGizmo)
                Gizmos.DrawWireCube(_ceilingCheck.position, new Vector3(-_ceilingCheckSize.x, _ceilingCheckSize.y, 1f));

            if (_showGroundCheckRayGizmos)
            {
                for (int i = 0; i < _groundCheckRayPoints.Length; i++)
                {
                    if (Application.isEditor && OnGroundRays[i] == true) Gizmos.color = Color.green;
                    else Gizmos.color = Color.red;

                    Gizmos.DrawLine(_groundCheckRayPoints[i].position, _groundCheckRayPoints[i].position + (Vector3.down * _groundCheckRayLength));
                }
            }

            if (_showWallCheckRayGizmos)
            {
                for (int i = 0; i < _wallCheckRayPoints.Length; i++)
                {
                    Gizmos.color = OnWallRays[i] ? Color.green : Color.red;
                    Gizmos.DrawLine(_wallCheckRayPoints[i].position, _wallCheckRayPoints[i].position + (transform.right * _wallCheckRayLength));
                }
            }

            if (_showCeilingCheckRayGizmos)
            {
                for (int i = 0; i < _ceilingCheckRayPoints.Length; i++)
                {
                    Gizmos.color = OnCeilingRays[i] ? Color.green : Color.red;
                    Gizmos.DrawLine(_ceilingCheckRayPoints[i].position, _ceilingCheckRayPoints[i].position + (Vector3.up * _ceilingCheckRayLength));
                }
            }
        }
    }
}
