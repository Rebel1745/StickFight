using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StickFight
{
    public class Kick : MonoBehaviour
    {
        [SerializeField] private float _KickDuration = 0.1f;
        private float _currentDuration;

        private Animator _anim;
        private Controller _controller;

        private bool _isKicking, _isKickingInput, _isDashingInput, _isKickingMuted, _isDashingMuted, _isInputMuted;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _controller = GetComponent<Controller>();
        }

        private void Update()
        {
            _isInputMuted = _controller.input.RetrieveIsMutedInput();
            _isKickingInput = _controller.input.RetrieveKickInput(false);
            _isDashingInput = _controller.input.RetrieveDashInput(false);
            _isKickingMuted = _controller.input.RetrieveKickInput(true);
            _isDashingMuted = _controller.input.RetrieveDashInput(true);

            if (!_isKicking && _isKickingInput)
            {
                StartKicking();
            }

            UpdateKicking();

            if (_currentDuration >= _KickDuration)
            {
                StopKicking();
            }
        }

        private void StartKicking()
        {
            _isKicking = true;
            _currentDuration = 0f;

            _anim.SetBool("isKicking", true);
        }

        private void UpdateKicking()
        {
            _currentDuration += Time.deltaTime;
        }

        private void StopKicking()
        {
            _isKicking = false;
            _anim.SetBool("isKicking", false);
            _controller.input.KickFinished();
        }
    }
}
