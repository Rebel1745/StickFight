using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StickFight
{
    public class Kick : MonoBehaviour
    {
        [Header("Kicking")]
        [SerializeField] private float _kickDuration = 0.2f;
        private float _currentDuration;
        [SerializeField] private LayerMask _whatIsEnemy;
        private bool _checkForHit = true;

        [Header("Standard Kick")]
        [SerializeField, Tooltip("Location of the center of the box that checks hits for standard kicks")] private Transform _hitCheckOriginStandardKick;
        [SerializeField] private Vector2 _hitBoxSizeStandardKick;
        [SerializeField] private bool _showStandardKickGizmo = false;

        [Header("Air Kick")]
        [SerializeField, Tooltip("Location of the center of the box that checks hits for standard kicks while jumping")] private Transform _hitCheckOriginAirKick;
        [SerializeField] private Vector2 _hitBoxSizeAirKick;
        [SerializeField] private bool _showAirKickGizmo = false;
        [SerializeField] private int _maximumAirKicks = 2;
        private int _currentAirKicks = 0;

        [Header("Dash Kick")]
        [SerializeField, Tooltip("Location of the center of the box that checks hits for dash kicks")] private Transform _hitCheckOriginDashKick;
        [SerializeField] private Vector2 _hitBoxSizeDashKick;
        [SerializeField] private bool _showDashKickGizmo = false;

        private Animator _anim;
        private Controller _controller;
        private Gravity _gravity;
        private CollisionDataRetriever _collisionDataRetriever;

        private bool _isKicking, _isKickingInput, _isDashingInput, _isDashingMutedInput, _isKickingMutedInput, _onGround;
        private KickType _kickType;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _controller = GetComponent<Controller>();
            _gravity = GetComponent<Gravity>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        }

        private void Update()
        {
            _isKickingInput = _controller.input.RetrieveKickInput(false);
            _isDashingInput = _controller.input.RetrieveDashInput(false);
            _isDashingMutedInput = _controller.input.RetrieveDashInput(true);
            _isKickingMutedInput = _controller.input.RetrieveKickInput(true);
            _onGround = _collisionDataRetriever.OnGround;

            if (_onGround)
                _currentAirKicks = 0;

            if (!_isKicking && _isKickingInput && (!_isDashingInput && !_isDashingMutedInput))
            {
                if (_onGround)
                    StartStandardKick();
                else if (_currentAirKicks < _maximumAirKicks)
                    StartAirKick();
            }
            else if (!_isKicking && _isKickingMutedInput && _isDashingInput)
            {
                StartDashKick();
            }

            if (_isKicking)
                UpdateKicking();
        }

        private void StartAirKick()
        {
            _anim.SetBool("isKicking", true);
            _currentAirKicks++;
            _isKicking = true;
            _kickType = KickType.Air;
            _currentDuration = 0f;

        }

        private void StartStandardKick()
        {
            print("StartKick");
            _anim.SetBool("isKicking", true);
            _isKicking = true;
            _kickType = KickType.Standard;
            _currentDuration = 0f;
        }

        private void StartDashKick()
        {
            _isKicking = true;
            _kickType = KickType.Dash;

            _anim.SetBool("isKicking", true);
        }

        private void UpdateKicking()
        {
            if (_checkForHit)
                CheckForHit();

            if (_kickType == KickType.Standard || _kickType == KickType.Air)
            {
                _currentDuration += Time.deltaTime;
                if (_currentDuration >= _kickDuration)
                    StopKicking();
            }

            if (_kickType == KickType.Dash)
            {
                if (!_isDashingInput)
                    StopKicking();
            }
        }

        private void StopKicking()
        {
            _isKicking = false;
            _checkForHit = true;
            _anim.SetBool("isKicking", false);
            _gravity.ResetToDefaultGravity(true, false, "Kick::CheckForHit()");
            _controller.input.KickFinished();
        }

        private void CheckForHit()
        {
            Collider2D[] hits;

            if (_kickType == KickType.Standard)
                hits = Physics2D.OverlapBoxAll(_hitCheckOriginStandardKick.position, _hitBoxSizeStandardKick, 0f, _whatIsEnemy);
            else if (_kickType == KickType.Air)
                hits = Physics2D.OverlapBoxAll(_hitCheckOriginAirKick.position, _hitBoxSizeAirKick, 0f, _whatIsEnemy);
            else
                return; // we dont check for hits with the dash, only standard and air kicks

            if (hits.Length > 0)
            {
                // if we hit an enemy, suspend gravity so we can hit again
                _gravity.ZeroGravity(true, true, "Kick::CheckForHit()");
                foreach (Collider2D c in hits)
                {
                    print(_kickType.ToString() + " collided with " + c.name);
                }
                _checkForHit = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (_showStandardKickGizmo)
                Gizmos.DrawWireCube(_hitCheckOriginStandardKick.position, new Vector3(_hitBoxSizeStandardKick.x, _hitBoxSizeStandardKick.y, 1f));
            if (_showAirKickGizmo)
                Gizmos.DrawWireCube(_hitCheckOriginAirKick.position, new Vector3(_hitBoxSizeAirKick.x, _hitBoxSizeAirKick.y, 1f));
            if (_showDashKickGizmo)
                Gizmos.DrawWireCube(_hitCheckOriginDashKick.position, new Vector3(_hitBoxSizeDashKick.x, _hitBoxSizeDashKick.y, 1f));
        }
    }

    public enum KickType
    {
        Standard,
        Dash,
        Air
    }
}
