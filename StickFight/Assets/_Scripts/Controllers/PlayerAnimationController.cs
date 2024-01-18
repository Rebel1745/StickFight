using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StickFight
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator _anim;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _body;
        private AnimationToPlay _animationState;
        private AnimationToPlay _previousAnimation;
        private Dictionary<AnimationToPlay, string> _animationDictionary;
        bool _isFacingRight = true;

        // animation names to add to our dictionary of animation types
        [SerializeField] string IdleAnimationName = "Idle";
        [SerializeField] string RunAnimationName = "Run";
        [SerializeField] string JumpAnimationName = "Jump";
        [SerializeField] string DashAnimationName = "Dash";
        [SerializeField] string DashPunchAnimationName = "DashPunch";
        [SerializeField] string DashKickAnimationName = "DashKick";
        [SerializeField] string PunchAnimationName = "Punch";
        [SerializeField] string KickAnimationName = "Kick";
        [SerializeField] string WallSlideAnimationName = "WallSlide";
        [SerializeField] string WallClimbAnimationName = "WallClimb";
        [SerializeField] string CeilingMoveAnimationName = "WallClimb";

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _body = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            ConfigureAnimationDictionary();
        }

        private void Update()
        {
            CheckCharacterDirection();
        }

        void CheckCharacterDirection()
        {
            if (_animationState == AnimationToPlay.Dash || _animationState == AnimationToPlay.DashPunch || _animationState == AnimationToPlay.DashKick)
                return;

            if (_body.velocity.x > 0f && !_isFacingRight)
                Flip();
            else if (_body.velocity.x < 0f && _isFacingRight)
                Flip();
        }

        public void ChangeAnimationState(AnimationToPlay animationToPlay)
        {
            if (animationToPlay == _previousAnimation)
                return;

            _animationState = animationToPlay;

            _anim.Play(_animationDictionary[_animationState]);

            _previousAnimation = animationToPlay;
        }

        void ConfigureAnimationDictionary()
        {
            _animationDictionary = new Dictionary<AnimationToPlay, string>();
            _animationDictionary.Add(AnimationToPlay.Idle, IdleAnimationName);
            _animationDictionary.Add(AnimationToPlay.Run, RunAnimationName);
            _animationDictionary.Add(AnimationToPlay.Jump, JumpAnimationName);
            _animationDictionary.Add(AnimationToPlay.Dash, DashAnimationName);
            _animationDictionary.Add(AnimationToPlay.DashPunch, DashPunchAnimationName);
            _animationDictionary.Add(AnimationToPlay.DashKick, DashKickAnimationName);
            _animationDictionary.Add(AnimationToPlay.Punch, PunchAnimationName);
            _animationDictionary.Add(AnimationToPlay.Kick, KickAnimationName);
            _animationDictionary.Add(AnimationToPlay.WallSlide, WallSlideAnimationName);
            _animationDictionary.Add(AnimationToPlay.WallClimb, WallClimbAnimationName);
            _animationDictionary.Add(AnimationToPlay.CeilingMove, CeilingMoveAnimationName);
        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            _isFacingRight = !_isFacingRight;

            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }
    }
}

public enum AnimationToPlay
{
    Idle,
    Run,
    Jump,
    Dash,
    DashPunch,
    DashKick,
    Punch,
    Kick,
    WallSlide,
    WallClimb,
    CeilingMove
}