using UnityEngine;
using UnityEngine.InputSystem;

namespace StickFight
{
    [CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
    public class PlayerController : InputController
    {
        private PlayerInputActions _inputActions;
        private bool _isJumping, _isWallClimb, _isDashing, _isInputMuted, _isPunching, _isKicking;
        private int _dashDirection; // -1 = left, 0 = no dash, 1 = right

        private void OnEnable()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Gameplay.Enable();
            _isInputMuted = false;

            // jump
            _inputActions.Gameplay.Jump.started += JumpStarted;
            _inputActions.Gameplay.Jump.canceled += JumpCanceled;

            // wall climb
            _inputActions.Gameplay.WallClimb.started += WallClimbStarted;
            _inputActions.Gameplay.WallClimb.canceled += WallClimbCanceled;

            // dash
            _inputActions.Gameplay.DashLeft.started += DashLeftStarted;
            _inputActions.Gameplay.DashRight.started += DashRightStarted;

            // punch
            _inputActions.Gameplay.Punch.started += PunchStarted;

            // kick
            _inputActions.Gameplay.Kick.started += KickStarted;
        }

        private void OnDisable()
        {
            _inputActions.Gameplay.Disable();
            _isInputMuted = true;

            // jump
            _inputActions.Gameplay.Jump.started -= JumpStarted;
            _inputActions.Gameplay.Jump.canceled -= JumpCanceled;

            // wall climb
            _inputActions.Gameplay.WallClimb.started -= WallClimbStarted;
            _inputActions.Gameplay.WallClimb.canceled -= WallClimbCanceled;
            
            // dash
            _inputActions.Gameplay.DashLeft.started -= DashLeftStarted;
            _inputActions.Gameplay.DashRight.started -= DashRightStarted;

            // punch
            _inputActions.Gameplay.Punch.started -= PunchStarted;

            // kick
            _inputActions.Gameplay.Kick.started -= KickStarted;

            _inputActions = null;
        }

        // TODO: make a function to limit movement for a time (while dashing, wall jumping etc)
        // _canMove = true;  void UpdateCanMove(bool canMove); void UpdateCanMove(bool canMove, float delay);
        public override Vector2 RetrieveMoveInput(bool includeMutedInput)
        {
            if (_isInputMuted && !includeMutedInput) return Vector2.zero;

            return _inputActions.Gameplay.Move.ReadValue<Vector2>();
        }

        private void JumpCanceled(InputAction.CallbackContext context)
        {
            _isJumping = false;
        }

        private void JumpStarted(InputAction.CallbackContext context)
        {
            _isJumping = true;
        }

        public override bool RetrieveJumpInput(bool includeMutedInput)
        {
            if (_isInputMuted && !includeMutedInput) return false;

            return _isJumping;
        }

        private void WallClimbCanceled(InputAction.CallbackContext context)
        {
            _isWallClimb = false;
        }

        private void WallClimbStarted(InputAction.CallbackContext context)
        {
            _isWallClimb = true;
        }

        public override bool RetrieveWallClimbInput(bool includeMutedInput)
        {
            if (_isInputMuted && !includeMutedInput) return false;

            return _isWallClimb;
        }

        private void DashLeftStarted(InputAction.CallbackContext context)
        {
            _isDashing = true;
            _dashDirection = -1;

            // FYI: in the InputActions, both dash left and right values are passed not as 'Button' (as jump is)
            // but as a value, clamped to the nearest integer (i.e 0 or 1)
            // This is to stop multiple dash calls which seem to be triggered otherwise
        }

        private void DashRightStarted(InputAction.CallbackContext context)
        {
            _isDashing = true;
            _dashDirection = 1;
        }

        public override void DashFinished()
        {
            _isDashing = false;
            _dashDirection = 0;
        }

        public override bool RetrieveDashInput(bool includeMutedInput)
        {
            if (_isInputMuted && !includeMutedInput) return false;

            return _isDashing;
        }

        public override int RetrieveDashDirection(bool includeMutedInput)
        {
            if (_isInputMuted && !includeMutedInput) return 0;

            return _dashDirection;
        }

        public override void UpdateInputMuting(bool isMuted)
        {
            _isInputMuted = isMuted;
        }

        public override void UpdateInputMuting(bool isMuted, float duration)
        {
            // wait and see if we will need this function
        }

        public override bool RetrieveIsMutedInput()
        {
            return _isInputMuted;
        }

        private void PunchStarted(InputAction.CallbackContext context)
        {
            _isPunching = true;
        }

        public override bool RetrievePunchInput(bool includeMutedInput)
        {
            if (_isInputMuted && !includeMutedInput) return false;

            return _isPunching;
        }

        public override void PunchFinished()
        {
            _isPunching = false;
        }

        private void KickStarted(InputAction.CallbackContext context)
        {
            _isKicking = true;
        }

        public override bool RetrieveKickInput(bool includeMutedInput)
        {
            if (_isInputMuted && !includeMutedInput) return false;

            return _isKicking;
        }

        public override void KickFinished()
        {
            _isKicking = false;
        }
    }
}
