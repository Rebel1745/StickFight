using UnityEngine;

namespace StickFight
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class WallInteractor : MonoBehaviour
    {
        public bool WallJumping { get; private set; }

        [Header("Wall Slide")]
        [SerializeField][Range(0.1f, 5f)] private float _wallSlideMaxSpeed = 2f;
        [Header("Wall Climb")]
        [SerializeField][Range(2f, 10f)] private float _wallClimbMaxSpeed = 4f;
        
        private Animator _anim;
        private CollisionDataRetriever _collisionDataRetriever;
        private Rigidbody2D _body;
        private Controller _controller;

        private Vector2 _velocity, _moveInput;
        private bool _onWall, _onGround, _onCeiling, _jumpInput, _isClinging, _isDashing, _isInputMuted;
        private float _wallDirectionX, _gravityScale, _initialGravityScale;
        private bool[] _onWallRays, _onCeilingRays;
        
        void Start()
        {
            _anim = GetComponent<Animator>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _body = GetComponent<Rigidbody2D>();
            _controller = GetComponent<Controller>();

            _initialGravityScale = _body.gravityScale;
        }
        
        void Update()
        {
            RetrieveInputsAndCollisions();
        }

        private void FixedUpdate()
        {
            ProcessWallInteraction();
        }

        void RetrieveInputsAndCollisions()
        {
            _isInputMuted = _controller.input.RetrieveIsMutedInput();
            _jumpInput = _controller.input.RetrieveJumpInput(false);
            _isClinging = _controller.input.RetrieveWallClimbInput(false);
            _isDashing = _controller.input.RetrieveDashInput(false);
            _moveInput = _controller.input.RetrieveMoveInput(false);

            _onWall = _collisionDataRetriever.OnWall;

            _velocity = _body.velocity;
            _gravityScale = _body.gravityScale;
            _onGround = _collisionDataRetriever.OnGround;
            _onCeiling = _collisionDataRetriever.OnCeiling;
            _wallDirectionX = _collisionDataRetriever.ContactNormal.x;

            _onWallRays = _collisionDataRetriever.OnWallRays;
            _onCeilingRays = _collisionDataRetriever.OnCeilingRays;
        }

        void ProcessWallInteraction()
        {
            // if we are not in control, we can't do anything on the wall
            if (_isInputMuted) return;

            // if we have pressed jump, we don't need to bother about the rest of the wall code
            if (_jumpInput)
            {
                ResetJumpAnimVariables();
                return;
            }

            if (_onWall)
            {
                _anim.SetBool("isWall", true);
                // if we are on a wall, we are not jumping
                _anim.SetBool("isJumping", false);

                // Wall Stick - if we are clinging to the wall, but not pressing a direction, stay still
                if (_isClinging && (_moveInput.y > -0.1f && _moveInput.y < 0.1f ) && !_onGround)
                    WallStick();

                // Wall Climb - if we are clinging, and pressing up, we climb
                if(_isClinging && _moveInput.y > 0.1f)
                    WallClimb();

                // Wall Slide
                if(((_isClinging && _moveInput.y < -.01f) || !_isClinging) && !_onGround)
                    WallSlide();

                // Ceiling Interaction - if we are on a wall, but touching the ceiling
                if(_onCeiling && _isClinging)
                    CeilingInteraction();

                // check to see if we are peeking over a wall onto a platform
                CheckWallEdge();

            }
            else if (_onCeiling)
            {
                if(_isClinging)
                    CeilingInteraction();
            }
            else
            {
                ResetJumpAnimVariables();
            }
        }

        private void CheckWallEdge()
        {
            // if we are only touching the wall with the lowest raycast, take control from the player and move them to the platform above
            if(!_onWallRays[1] && _onWallRays[2])
            {
                print("Auto move to platform above");
            }

            // if we are only touching the wall with the highest raycast, take control from the player and move them to the ceiling below them
            if(!_onWallRays[1] && _onWallRays[0])
            {
                print("Auto move to ceiling below");
            }
        }

        private void CheckCeilingEdge()
        {
            // if we are only touching the ceiling with the leftmost (when facing right) raycast, take control from the player and move them to the wall above
            if(!_onCeilingRays[1] && _onCeilingRays[0])
            {
                print("Auto move to wall above");
            }
        }

        void ResetJumpAnimVariables()
        {
            _anim.SetBool("isWall", false);
            _anim.SetBool("isCeiling", false);
            _anim.SetBool("isClinging", false);

            if (!_onGround)
                _anim.SetBool("isJumping", true);
        }

        void WallStick()
        {
            _anim.SetBool("isClinging", true);
            _anim.SetBool("isCeiling", false);
            _gravityScale = 0f;
            _velocity.x = 0f;
            _velocity.y = 0f;
            ApplyGravityAndVelocity();
        }

        void WallClimb()
        {
            _anim.SetBool("isClinging", true);
            _anim.SetBool("isCeiling", false);
            _gravityScale = 0f;
            _velocity.x = 0f;
            _velocity.y = _moveInput.y * _wallClimbMaxSpeed;
            ApplyGravityAndVelocity();
        }

        void WallSlide()
        {
            _anim.SetFloat("ClimbSpeed", _body.velocity.y);
            _anim.SetBool("isCeiling", false);
            _anim.SetBool("isClinging", false);
            _gravityScale = _initialGravityScale;
            _velocity.y = -_wallSlideMaxSpeed;
            ApplyGravityAndVelocity();
        }

        void CeilingInteraction()
        {
            _anim.SetBool("isCeiling", true);
            _anim.SetBool("isJumping", false);
            _gravityScale = 0f;
            _velocity.x = _moveInput.x * _wallClimbMaxSpeed;
            _velocity.y = 0f;

            if (_onWall)
            {
                // we are at the top of a wall, also touching the ceiling.  This should also allow movement on the y axis
                _velocity.y = _moveInput.y * _wallClimbMaxSpeed;
            }
            else
            {
                _anim.SetBool("isWall", false);
            }

            ApplyGravityAndVelocity();
            CheckCeilingEdge();
        }

        void ApplyGravityAndVelocity()
        {
            _body.velocity = _velocity;
            _body.gravityScale = _gravityScale;
        }
    }
}
