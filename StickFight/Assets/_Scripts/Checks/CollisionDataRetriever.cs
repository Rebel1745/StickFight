using UnityEngine;

namespace StickFight
{
    public class CollisionDataRetriever : MonoBehaviour
    {
        public bool OnGround { get; private set; }
        public bool OnWall { get; private set; }
        public bool OnCeiling { get; private set; }
        public float Friction { get; private set; }
        public Vector2 ContactNormal { get; private set; }

        private PhysicsMaterial2D _material;
        
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private Vector2 _groundCheckSize;
        [SerializeField] private LayerMask _whatIsGround;

        [SerializeField] private Transform _wallCheck;
        [SerializeField] private Vector2 _wallCheckSize;
        [SerializeField] private LayerMask _whatIsWall;

        [SerializeField] private Transform _ceilingCheck;
        [SerializeField] private Vector2 _ceilingCheckSize;
        [SerializeField] private LayerMask _whatIsCeiling;

        private void FixedUpdate()
        {
            CheckForGroundContact();
            CheckForWallContact();
            CheckForCeilingContact();
        }

        void CheckForGroundContact()
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
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            OnGround = false;
            Friction = 0;
            OnWall = false;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
                Gizmos.DrawWireCube(_groundCheck.position, new Vector3(_groundCheckSize.x, _groundCheckSize.y, 1f));
                Gizmos.DrawWireCube(_wallCheck.position, new Vector3(_wallCheckSize.x, _wallCheckSize.y, 1f));
                Gizmos.DrawWireCube(_ceilingCheck.position, new Vector3(-_ceilingCheckSize.x, _ceilingCheckSize.y, 1f));
        }
    }
}
