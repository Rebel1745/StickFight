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
        private bool _onWall, _onGround, _onCeiling, _jumpInput, _isClinging, _isDashing, _isInputMuted, _isAutoMoveToPlatform, _isAutoMoveToCeiling, _isAutoMoveToWall, _autoFlipped;
        private float _wallDirectionX, _gravityScale, _initialGravityScale;
        private bool[] _onWallRays, _onCeilingRays, _onGroundRays;

        private Move _move;
        
        void Start()
        {
            _anim = GetComponent<Animator>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _body = GetComponent<Rigidbody2D>();
            _controller = GetComponent<Controller>();
            _move = GetComponent<Move>();

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
            _onGroundRays = _collisionDataRetriever.OnGroundRays;
        }

        void ProcessWallInteraction()
        {
            // check to see if we are peeking over a wall onto a platform
            if (_isAutoMoveToPlatform)
                MoveToPlatformAbove();
            else
                CheckWallEdge();

            if (_isAutoMoveToCeiling)
                MoveToCeilingBelow();
            else
                CheckWallEdge();

            if (_isAutoMoveToWall)
                MoveToWallAbove();
            else
                CheckCeilingEdge();

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

        private void MoveToWallAbove()
        {
            Vector3 dir = _move.IsFacingRight ? transform.right : -transform.right;
            if (_onCeilingRays[0])
            {
                // if we are still on the ceiling move the player forward until they are fully over the edge
                _body.velocity = dir * _wallClimbMaxSpeed;
                print("MoveToWallAbove():: Moving across");
                return;
            }
            if (!_onWallRays[1])
            {
                // if we arent facing the wall, flip the sprite
                if (!_autoFlipped)
                {
                    _move.Flip();
                    _autoFlipped = true;
                }

                // if the middle ray is not touching the wall, move up until it is
                _body.velocity = transform.up * _wallClimbMaxSpeed;
                _controller.input.UpdateInputMuting(false);
                print("MoveToWallAbove():: Moving up");
                return;
            }

            _isAutoMoveToWall = false;
            _controller.input.UpdateInputMuting(false);
        }

        // TODO: THIS BREAKS IF A DIFFERENT DIRECTION THAN INTENDED IS BEING PRESSED.
        // E.G. IF WE WANT TO MOVE UP AND RIGHT AND LEFT AND UP IS BEING PRESSED, THE PLAYER SOMETIMES FLOATS TO THE LEFT.
        // This could be possibly fixed by defining the dir before calling this function in the CheckWallEdge function
        private void MoveToPlatformAbove()
        {
            Vector3 dir = _move.IsFacingRight ? transform.right : -transform.right;
            // if we are still touching the wall with the lowest ray, move the player up
            if (_onWallRays[2])
            {
                _body.velocity = transform.up * _wallClimbMaxSpeed;
                print("MoveToPlatformAbove():: Moving up");
                return;
            }
            if (!_onGroundRays[2])
            {
                _body.velocity = dir * _wallClimbMaxSpeed;
                print("MoveToPlatformAbove():: Moving across");
                _controller.input.UpdateInputMuting(false);
                return;
            }

            _isAutoMoveToPlatform = false;
            _controller.input.UpdateInputMuting(false);
        }

        private void MoveToCeilingBelow()
        {
            Vector3 dir = _move.IsFacingRight ? transform.right : -transform.right;
            // if we are still touching the wall with the lowest ray, move the player up
            if (_onWallRays[0])
            {
                //print("MoveToCeilingBelow():: Moving up");
                _body.velocity = -transform.up * _wallClimbMaxSpeed;
                return;
            }
            if (!_onCeilingRays[2])
            {
                //print("MoveToCeilingBelow():: Moving across");
                _body.velocity = dir * _wallClimbMaxSpeed;
                _controller.input.UpdateInputMuting(false);
                return;
            }

            _isAutoMoveToCeiling = false;
            _controller.input.UpdateInputMuting(false);
        }

        private void CheckWallEdge()
        {
            // if we are only touching the wall with the lowest raycast, take control from the player and move them to the platform above
            if(_onWall && _isClinging && !_onWallRays[1] && _onWallRays[2] && _moveInput.y > 0.1f)
            {
                //print("Auto move to platform above");
                _isAutoMoveToPlatform = true;
                _body.velocity = Vector2.zero;
                // mute the input so control is out of the players hands
                _controller.input.UpdateInputMuting(true);
            }

            // if we are only touching the wall with the highest raycast, take control from the player and move them to the ceiling below them
            if(_onWall && _isClinging && !_onWallRays[1] && _onWallRays[0] && _moveInput.y < -0.1f)
            {
                //print("Auto move to ceiling below");
                _isAutoMoveToCeiling = true;
                _body.velocity = Vector2.zero;
                // mute the input so control is out of the players hands
                _controller.input.UpdateInputMuting(true);
            }
        }

        private void CheckCeilingEdge()
        {
            // if we are only touching the ceiling with the leftmost (when facing right) raycast, take control from the player and move them to the wall above
            if(_onCeiling && _isClinging && !_onCeilingRays[1] && _onCeilingRays[0] && _moveInput.y > 0.1f)
            {
                //print("Auto move to wall above");
                _isAutoMoveToWall = true;
                _autoFlipped = false;
                _body.velocity = Vector2.zero;
                // mute the input so control is out of the players hands
                _controller.input.UpdateInputMuting(true);
            }
        }

        void ResetJumpAnimVariables()
        {
            _anim.SetBool("isWall", false);
            _anim.SetBool("isCeiling", false);
            _anim.SetBool("isClinging", false);

            if (!_onGround && !_isInputMuted)
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
        }

        void ApplyGravityAndVelocity()
        {
            _body.velocity = _velocity;
            _body.gravityScale = _gravityScale;
        }
    }
}
