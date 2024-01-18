using UnityEngine;

namespace StickFight
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Move : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
        [SerializeField] bool _useAcceleration = true;
        [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
        [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

        private Animator _anim;
        private Controller _controller;
        private Vector2 _direction, _desiredVelocity, _velocity;
        private Rigidbody2D _body;
        private CollisionDataRetriever _collisionDataRetriever;
        private AnimationController _playerAnimationController;

        private float _maxSpeedChange, _acceleration;
        private bool _onGround, _onWall;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _body = GetComponent<Rigidbody2D>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();
            _playerAnimationController = GetComponent<AnimationController>();
        }

        private void Update()
        {
            _direction.x = _controller.input.RetrieveMoveInput(this.gameObject).x;
            _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _collisionDataRetriever.Friction, 0f);
        }

        private void FixedUpdate()
        {
            _onGround = _collisionDataRetriever.OnGround;

            _velocity = _body.velocity;
           
            if (_useAcceleration)
            {
                _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
                _maxSpeedChange = _acceleration * Time.deltaTime;
                _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
            }                
            else
                _velocity.x = _desiredVelocity.x;

            _body.velocity = _velocity;

            _anim.SetBool("isGround", _onGround);
            _anim.SetFloat("MovementSpeed", Mathf.Abs(_velocity.x));

            if (_onGround)
            {
                if (_velocity.x == 0f)
                {
                    _playerAnimationController.ChangeAnimationState(AnimationToPlay.Idle);
                }
                else
                {
                    _playerAnimationController.ChangeAnimationState(AnimationToPlay.Run);
                }
            }
        }
    }
}
